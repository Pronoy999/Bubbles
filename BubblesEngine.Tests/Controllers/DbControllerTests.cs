using System;
using System.IO;
using BubblesEngine.Controllers.Implementation;
using BubblesEngine.Engines;
using BubblesEngine.Exceptions;
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

        [Fact]
        public async void ShouldCreateANodeAndTypesFileAndFolderWhenValidGraphAndDatabaseIsPassed()
        {
            const string dbName = "some-valid-db";
            const string graphName = "some-valid-graph";
            const string nodeData = "{data:Hello world}";

            var path = Environment.GetEnvironmentVariable(Constants.DbRootFolderKey) + Path.DirectorySeparatorChar +
                       dbName + Path.DirectorySeparatorChar + graphName;
            _fileWrapper.Setup(fs => fs.IsExists(It.Is<string>(x => x == path))).Returns(true);
            _fileWrapper.Setup(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.Is<string>(x => x != path))).Returns(false);
            _fileWrapper.Setup(fs => fs.CreateFolder(It.IsAny<string>())).Returns(true);

            var result = await _controller.CreateNode(dbName, graphName, "Person", nodeData);

            Assert.True(result);
            _fileWrapper.Verify(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            _fileWrapper.Verify(fs => fs.CreateFolder(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void ShouldCreateANodeWhenTypesFolderAlreadyExists()
        {
            const string dbName = "some-valid-db";
            const string graphName = "some-valid-graph";
            const string nodeData = "{data:Hello world}";
            const string typesData = "{\"NodeIds\":[\"guid-1\",\"guid-2\"],\"TypeName\":\"Person\"}";

            var path = Environment.GetEnvironmentVariable(Constants.DbRootFolderKey) + Path.DirectorySeparatorChar +
                       dbName + Path.DirectorySeparatorChar + graphName;
            _fileWrapper.Setup(fs => fs.IsExists(path)).Returns(true);
            _fileWrapper.Setup(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.CreateFolder(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.GetFileContents(It.IsAny<string>())).ReturnsAsync(typesData);

            var result = await _controller.CreateNode(dbName, graphName, "Person", nodeData);

            Assert.True(result);
            _fileWrapper.Verify(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            _fileWrapper.Verify(fs => fs.CreateFolder(It.IsAny<string>()), Times.Never);
            _fileWrapper.Verify(fs => fs.GetFileContents(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void ShouldNotCreateANodeWhenGraphDoesNotExists()
        {
            const string dbName = "some-valid-db";
            const string graphName = "some-valid-graph";
            var path = Environment.GetEnvironmentVariable(Constants.DbRootFolderKey) + Path.DirectorySeparatorChar +
                       dbName + Path.DirectorySeparatorChar + graphName;
            _fileWrapper.Setup(fs => fs.IsExists(path)).Returns(false);

            await Assert.ThrowsAsync<BubblesException>(() =>
                _controller.CreateNode(dbName, graphName, "Person", "{}"));

            _fileWrapper.Verify(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _fileWrapper.Verify(fs => fs.CreateFolder(It.IsAny<string>()), Times.Never);
        }
    }
}