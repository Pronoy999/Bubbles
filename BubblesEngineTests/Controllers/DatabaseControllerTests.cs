using System;
using System.Collections.Generic;
using BubblesEngine.Controllers.Implementation;
using BubblesEngine.Engines;
using BubblesEngine.Exceptions;
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

        #region GetDatabase

        [Fact]
        public void ShouldGetDatabaseWithValidData()
        {
            var listOfGraphs = new List<string>()
            {
                "graph-1", "graph-2"
            };
            _fileWrapper.Setup(f => f.IsDirectoryExists(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(f => f.GetDirectories(It.IsAny<string>())).Returns(listOfGraphs);
            const string databaseName = "some-database";
            const string userId = "some-user-id";

            var result = _controller.GetDatabase(databaseName, userId);

            Assert.NotNull(result);
            Assert.Equal(databaseName, result.DatabaseName);
            Assert.Equal(listOfGraphs[0], result.Graphs[0].GraphName);
        }

        [Fact]
        public void ShouldThrowExceptionWhenDatabaseNotFound()
        {
            _fileWrapper.Setup(f => f.IsDirectoryExists(It.IsAny<string>())).Returns(false);
            const string databaseName = "some-database";
            const string userId = "some-user-id";

            Assert.Throws<BubblesException>(() => _controller.GetDatabase(databaseName, userId));
        }

        [Fact]
        public void ShouldThrowExceptionWhenNullDatabaseNamePassed()
        {
            const string databaseName = null;
            const string userId = "some-user-id";

            _fileWrapper.Verify(f => f.IsDirectoryExists(It.IsAny<string>()), Times.Never);
            Assert.Throws<BubblesException>(() => _controller.GetDatabase(databaseName, userId));
        }

        #endregion
    }
}