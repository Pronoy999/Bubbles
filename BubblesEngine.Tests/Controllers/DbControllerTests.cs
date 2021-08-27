using System;
using BubblesEngine.Controllers.Implementation;
using BubblesEngine.Engines;
using BubblesEngine.Helpers;
using Moq;
using Xunit;

namespace BubblesEngine.Tests.Controllers
{
    public class DbControllerTests
    {
        private readonly Mock<IFileWrapper> _fileWrapper;
        private readonly Mock<IDomainFs> _domainFs;
        private readonly DbController _controller;

        public DbControllerTests()
        {
            _domainFs = new Mock<IDomainFs>();
            _fileWrapper = new Mock<IFileWrapper>();
            _controller = new DbController(_fileWrapper.Object);
            Environment.SetEnvironmentVariable(Constants.DbRootFolderKey, "/some-folder");
            Environment.SetEnvironmentVariable(Constants.TypesFolderName, "types");
        }

        [Fact]
        public void ShouldCreateDatabaseWhenValidNameIsPassed()
        {
            _fileWrapper.Setup(fs => fs.CreateFolder(It.IsAny<string>())).Returns(true);
            var result = _controller.CreateDatabase("some-database");
            Assert.True(result);
        }

        [Fact]
        public void ShouldNotCreateDatabaseWhenInvalidNameIsPassed()
        {
            _fileWrapper.Setup(fs => fs.CreateFolder(null)).Returns(false);
            var result = _controller.CreateDatabase(null);
            Assert.False(result);
        }

        [Fact]
        public void ShouldCreateGraphWhenValidDatabaseIsPassed()
        {
            _fileWrapper.Setup(fs => fs.CreateFolder(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(false);
            var result = _controller.CreateGraph("some-graph", "some-database");
            Assert.True(result);
            _fileWrapper.Verify(fs => fs.CreateFolder(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ShouldNotCreateGraphWhenAlreadyExists()
        {
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.CreateFolder(It.IsAny<string>())).Returns(true);
            var result = _controller.CreateGraph("some-graph", "some-database");
            Assert.True(result);
            _fileWrapper.Verify(fs => fs.CreateFolder(It.IsAny<string>()), Times.Once);
            _domainFs.Verify(fs => fs.CreateDirectory(It.IsAny<string>()), Times.Never);
        }
    }
}