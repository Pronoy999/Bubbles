using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BubblesEngine.Controllers.Implementation;
using BubblesEngine.Engines;
using BubblesEngine.Exceptions;
using BubblesEngine.Helpers;
using BubblesEngine.Models;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace BubblesEngine.Tests.Controllers
{
    public class DbControllerTests
    {
        private readonly Mock<IFileWrapper> _fileWrapper;
        private readonly Mock<IDomainFs> _domainFs;
        private readonly DbController _controller;
        private readonly string someUserId = "some-user-id";

        public DbControllerTests()
        {
            _domainFs = new Mock<IDomainFs>();
            _fileWrapper = new Mock<IFileWrapper>();
            _controller = new DbController(_fileWrapper.Object);
            Environment.SetEnvironmentVariable(Constants.DbRootFolderKey, "/some-folder");
            Environment.SetEnvironmentVariable(Constants.TypesFolderName, "types");
        }

        #region CreateGraph

        [Fact]
        public void ShouldCreateGraphWhenValidDatabaseIsPassed()
        {
            _fileWrapper.Setup(fs => fs.CreateFolder(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(false);
            var result = _controller.CreateGraph("some-graph", "some-database", someUserId);
            Assert.True(result);
            _fileWrapper.Verify(fs => fs.CreateFolder(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ShouldNotCreateGraphWhenAlreadyExists()
        {
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.CreateFolder(It.IsAny<string>())).Returns(true);
            var result = _controller.CreateGraph("some-graph", "some-database", someUserId);
            Assert.True(result);
            _fileWrapper.Verify(fs => fs.CreateFolder(It.IsAny<string>()), Times.Once);
            _domainFs.Verify(fs => fs.CreateDirectory(It.IsAny<string>()), Times.Never);
        }

        #endregion
        

        #region GetGraph

        [Fact]
        public void ShouldReturnGraphWhenValidDatabaseAndGraphNameIsPassed()
        {
            const string graphName = "some-graph";
            const string databaseName = "some-db";

            var listOfNodes = new List<string>
            {
                "guid-1.json",
                "guid-2.json"
            };
            var expectedGraph = new Graph
            {
                GraphName = graphName,
                Nodes = new List<Node>
                {
                    new Node
                    {
                        Id = "guid-1"
                    },
                    new Node
                    {
                        Id = "guid-2"
                    }
                }
            };
            _fileWrapper.Setup(fs => fs.IsDirectoryExists(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.GetAllFilesNames(It.IsAny<string>())).Returns(listOfNodes);
            var actualGraph = _controller.GetGraph(databaseName, graphName, someUserId);
            var expected = JsonConvert.SerializeObject(expectedGraph);
            var actual = JsonConvert.SerializeObject(actualGraph);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldThrowExceptionWhenInvalidGraphNameIsPassed()
        {
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(false);
            Assert.Throws<BubblesException>(() => _controller.GetGraph(null, null, null));
        }

        #endregion

        

        #region SearchNodeById

        [Fact]
        public async void ShouldSearchForNodeWithValidId()
        {
            const string dbName = "some-db";
            const string nodeId = "some-node-id";
            var nodeFileLocation =
                "/some-folder/" + dbName + "/" + someUserId + "/graphs/some-graph/" + nodeId + ".json";
            var node = new Node
            {
                Id = nodeId,
                Type = "Node-Type",
                Data = "{}"
            };
            _fileWrapper.Setup(fs => fs.SearchFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(nodeFileLocation);
            _fileWrapper.Setup(fs => fs.GetFileContents(It.Is<string>(x => x == nodeFileLocation)))
                .ReturnsAsync(JsonConvert.SerializeObject(node));

            var result = await _controller.SearchNodeById(dbName, nodeId, someUserId);

            var expected = JsonConvert.SerializeObject(node);
            var actual = JsonConvert.SerializeObject(result);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ShouldThrowErrorWhenNodeNotFound()
        {
            _fileWrapper.Setup(fs => fs.SearchFiles(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
            await Assert.ThrowsAsync<BubblesException>(() =>
                _controller.SearchNodeById("some-db", "some-node-id", someUserId));
        }

        #endregion

        #region SearchNodeByData

        [Fact]
        public async void ShouldReturnDataWhenFoundInAnyNode()
        {
            var files = new List<string>
            {
                "/some-folder/guid-1.json",
                "some-folder/guid-2.json"
            };
            var node = new Node
            {
                Data = "{'key':'value'}",
                Id = "guid-2",
                Type = "Node-Type"
            };
            _fileWrapper.Setup(fs => fs.GetAllFiles(It.IsAny<string>())).Returns(files);
            _fileWrapper.Setup(fs => fs.GetFileContents(It.IsAny<string>()))
                .ReturnsAsync(JsonConvert.SerializeObject(node));

            var result = await _controller.SearchNodeByData("some-db", "value", someUserId);

            var actual = JsonConvert.SerializeObject(result);
            var expected = JsonConvert.SerializeObject(node);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void ShouldReturnNullWhenSearchIsNotMatched()
        {
            var files = new List<string>
            {
                "/some-folder/guid-1.json",
                "some-folder/guid-2.json"
            };
            var node = new Node
            {
                Data = "{'key':'value'}",
                Id = "guid-2",
                Type = "Node-Type"
            };
            _fileWrapper.Setup(fs => fs.GetAllFiles(It.IsAny<string>())).Returns(files);
            _fileWrapper.Setup(fs => fs.GetFileContents(It.IsAny<string>()))
                .ReturnsAsync(JsonConvert.SerializeObject(node));

            var result = await _controller.SearchNodeByData("some-db", "Hello", someUserId);

            Assert.Null(result);
        }

        #endregion
    }
}