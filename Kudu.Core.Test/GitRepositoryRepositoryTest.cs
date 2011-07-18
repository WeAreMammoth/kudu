            Assert.Throws<InvalidOperationException>(() => GitExeRepository.ConvertStatus("AG"));
            ChangeType addDeleted = GitExeRepository.ConvertStatus("AD");
            Assert.Equal(ChangeType.Deleted, addDeleted);
A  New File With Spaces.txt
            Assert.Equal(8, status.Count);

            Assert.Equal("New File With Spaces.txt", status[7].Path);
            Assert.Equal(ChangeType.Added, status[7].Status);
        }

        [Fact]
        public void PopulateStatusHandlesFilesWithSpaces() {
            string status = @"
A	New File
";
            ChangeSetDetail detail = new ChangeSetDetail();
            detail.Files["New File"] = new FileInfo();
            GitExeRepository.PopulateStatus(status.AsReader(), detail);

            Assert.Equal(ChangeType.Added, detail.Files["New File"].Status);
        [Fact]
        public void ParseDiffChunkHandlesFilesWithSpacesInName() {
            string diff = @"diff --git a/New File b/New File
new file mode 100644
index 0000000..261a6bf
--- /dev/null
+++ b/New File	
@@ -0,0 +1 @@
+Ayayayya
\ No newline at end of file";
            ChangeSetDetail detail = null;
            var diffChunk = GitExeRepository.ParseDiffChunk(diff.AsReader(), ref detail);
            Assert.False(diffChunk.Binary);
            Assert.Equal("New File", diffChunk.FileName);
            Assert.Equal(2, diffChunk.Lines.Count);
            Assert.Equal("+Ayayayya", diffChunk.Lines[1].Text.TrimEnd());
        }

        [Fact]
        public void ParseDiffFileName() {
            string singleCharFileName = GitExeRepository.ParseFileName("git --diff a/a b/a");
            string evenNumberFileName = GitExeRepository.ParseFileName("git --diff a/aa b/aa");
            string moreAmbiguous = GitExeRepository.ParseFileName("git --diff a/ b  b/ b ");
            string fileNameWithSpaces = GitExeRepository.ParseFileName("git --diff a/New File b/New File");
            string fileNameWithSlashB = GitExeRepository.ParseFileName("git --diff a/foo/bar/lib/a.dll b/foo/bar/lib/a.dll");
            string ambiguous = GitExeRepository.ParseFileName("diff --git a/Folder b/blah.txt b/Folder b/blah.txt");


            Assert.Equal("a", singleCharFileName);
            Assert.Equal("aa", evenNumberFileName);
            Assert.Equal("New File", fileNameWithSpaces);
            Assert.Equal("foo/bar/lib/a.dll", fileNameWithSlashB);
            Assert.Equal("Folder b/blah.txt", ambiguous);
            Assert.Equal(" b ", moreAmbiguous);
        }
