using System.Collections.Generic;
using System.IO;
using BubblesEngine.Engines;
using BubblesEngine.Engines.Implementations;
using Moq;
using Xunit;

namespace BubblesEngine.Tests.Engines
{
    public class FileWrapperTests
    {
        private readonly Mock<IDomainFs> _domainFs;
        private readonly FileWrapper _fileWrapper;

        public FileWrapperTests()
        {
            _domainFs = new Mock<IDomainFs>();
            _fileWrapper = new FileWrapper(_domainFs.Object);
        }

        [Fact]
        public async void ShouldCreateFileWithValidPath()
        {
            _domainFs.Setup(fs => fs.WriteFile(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            var result = await _fileWrapper.CreateFile("some-path", "some-content");
            Assert.True(result);
        }

        [Fact]
        public async void ShouldNotCreateAFileWithInvalidPath()
        {
            _domainFs.Setup(fs => fs.WriteFile(null, It.IsAny<string>())).ReturnsAsync(false);
            var result = await _fileWrapper.CreateFile(null, null);
            Assert.False(result);
        }

        [Fact]
        public async void ShouldReadContentsOfValidFile()
        {
            _domainFs.Setup(fs => fs.ReadFile(It.IsAny<string>())).ReturnsAsync("some-content");
            var result = await _fileWrapper.GetFileContents("some-path");
            Assert.Equal("some-content", result);
        }

        [Fact]
        public void ShouldCheckWhenAValidFileExists()
        {
            _domainFs.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(true);
            var result = _fileWrapper.IsExists("some-path");
            Assert.True(result);
        }

        [Fact]
        public void ShouldCheckWhenAnInvalidFilePathIsPassed()
        {
            _domainFs.Setup(fs => fs.IsExists(null)).Returns(false);
            var result = _fileWrapper.IsExists(null);
            Assert.False(result);
        }

        [Fact]
        public void ShouldCreateADirectoryWhenValidPathIsPassed()
        {
            _domainFs.Setup(fs => fs.CreateDirectory(It.IsAny<string>())).Returns(true);
            var result = _fileWrapper.CreateFolder("some-path");
            Assert.True(result);
        }

        [Fact]
        public void ShouldNotCreateADuplicateFolder()
        {
            _domainFs.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(true);
            _domainFs.Setup(fs => fs.CreateDirectory(It.IsAny<string>())).Returns(true);
            var result = _fileWrapper.CreateFolder("some-path");
            Assert.True(result);
            _domainFs.Verify(fs => fs.CreateDirectory(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void ShouldListFoldersInADirectoryWithValidPath()
        {
            var subFolders = new List<string>
            {
                "some-folder-1/some-sub-folder-1",
                "some-folder-2/some-sub-folder-2",
                "some-folder-3/some-sub-folder-2",
            };
            var expected = new List<string>
            {
                "some-sub-folder-1",
                "some-sub-folder-2",
                "some-sub-folder-2",
            };
            _domainFs.Setup(fs => fs.ListDirectories(It.IsAny<string>())).Returns(subFolders);
            var result = _fileWrapper.GetDirectories("some-path");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ShouldThrowErrorWhenInvalidPathIsPassed()
        {
            _domainFs.Setup(fs => fs.ListDirectories(It.IsAny<string>())).Throws(new DirectoryNotFoundException
                { Source = "Directory not found" });
            Assert.Throws<DirectoryNotFoundException>(() => _fileWrapper.GetDirectories("some-invalid-path"));
        }

        [Fact]
        public void ShouldListFilesWhenValidPathIsPassed()
        {
            var listOfFiles = new List<string>
            {
                "/some-path/guid-1.json",
                "/some-path/guid-2.json"
            };
            var expected = new List<string>
            {
                "guid-1.json",
                "guid-2.json"
            };
            _domainFs.Setup(fs => fs.ListFiles(It.IsAny<string>())).Returns(listOfFiles);
            var result = _fileWrapper.GetFiles("some-path");
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ShouldNotListFilesWhenInvalidPathIsPassed()
        {
            _domainFs.Setup(fs => fs.ListFiles(It.IsAny<string>())).Returns((List<string>?)null);
            var result = _fileWrapper.GetFiles(null);
            Assert.IsType<List<string>>(result);
        }
    }
}