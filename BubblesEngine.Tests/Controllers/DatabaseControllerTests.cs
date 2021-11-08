using System;
using BubblesEngine.Controllers.Implementation;
using BubblesEngine.Engines;
using BubblesEngine.Helpers;
using Moq;
using Xunit;

namespace BubblesEngine.Tests.Controllers
{
    public class DatabaseControllerTests
    {
        private readonly Mock<IFileWrapper> _fileWrapper;
        private readonly Mock<IDomainFs> _domainFs;
        private readonly DatabaseController _controller;
        private readonly string someUserId = "some-user-id";

        public DatabaseControllerTests()
        {
            _domainFs = new Mock<IDomainFs>();
            _fileWrapper = new Mock<IFileWrapper>();
            _controller = new DatabaseController(_fileWrapper.Object);
            Environment.SetEnvironmentVariable(Constants.DbRootFolderKey, "/some-folder");
            Environment.SetEnvironmentVariable(Constants.TypesFolderName, "types");
        }

        #region CreateDatabase

        [Fact]
        public void ShouldCreateDatabaseWhenValidNameIsPassed()
        {
            _fileWrapper.Setup(fs => fs.CreateFolder(It.IsAny<string>())).Returns(true);
            var result = _controller.CreateDatabase("some-database", someUserId);
            Assert.True(result);
        }

        [Fact]
        public void ShouldNotCreateDatabaseWhenInvalidNameIsPassed()
        {
            _fileWrapper.Setup(fs => fs.CreateFolder(null)).Returns(false);
            var result = _controller.CreateDatabase(null, someUserId);
            Assert.False(result);
        }

        #endregion
    }
}