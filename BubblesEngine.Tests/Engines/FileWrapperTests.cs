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
        }
    }
}