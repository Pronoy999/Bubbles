using System;
using System.Collections.Generic;
using System.IO;
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

        #region CreateNode

        [Fact]
        public async void ShouldCreateANodeAndTypesFileAndFolderWhenValidGraphAndDatabaseIsPassed()
        {
            const string dbName = "some-valid-db";
            const string graphName = "some-valid-graph";
            const string nodeData = "{data:Hello world}";

            var path = Environment.GetEnvironmentVariable(Constants.DbRootFolderKey) + Path.DirectorySeparatorChar +
                       someUserId + Path.DirectorySeparatorChar + dbName + Path.DirectorySeparatorChar +
                       Constants.GraphFolderName + Path.DirectorySeparatorChar +
                       graphName;
            _fileWrapper.Setup(fs => fs.IsDirectoryExists(It.Is<string>(x => x == path))).Returns(true);
            _fileWrapper.Setup(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.Is<string>(x => x != path))).Returns(false);
            _fileWrapper.Setup(fs => fs.CreateFolder(It.IsAny<string>())).Returns(true);

            var result = await _controller.CreateNode(dbName, graphName, "Person", nodeData, someUserId);

            Assert.NotNull(result);
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
                       someUserId + Path.DirectorySeparatorChar + dbName + Path.DirectorySeparatorChar +
                       Constants.GraphFolderName + Path.DirectorySeparatorChar +
                       graphName;
            var typePath = path + Path.DirectorySeparatorChar + "types";
            _fileWrapper.Setup(fs => fs.IsDirectoryExists(It.Is<string>(x => x == path))).Returns(true);
            _fileWrapper.Setup(fs => fs.IsDirectoryExists(It.Is<string>(x => x == typePath))).Returns(true);
            _fileWrapper.Setup(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.CreateFolder(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.GetFileContents(It.IsAny<string>())).ReturnsAsync(typesData);

            var result = await _controller.CreateNode(dbName, graphName, "Person", nodeData, someUserId);

            Assert.NotNull(result);
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
                       dbName + Path.DirectorySeparatorChar + someUserId + Path.DirectorySeparatorChar +
                       Constants.GraphFolderName + Path.DirectorySeparatorChar +
                       graphName;
            _fileWrapper.Setup(fs => fs.IsExists(path)).Returns(false);

            await Assert.ThrowsAsync<BubblesException>(() =>
                _controller.CreateNode(dbName, graphName, "Person", "{}", someUserId));

            _fileWrapper.Verify(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _fileWrapper.Verify(fs => fs.CreateFolder(It.IsAny<string>()), Times.Never);
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

        #region GetNode

        [Fact]
        public async void ShouldReturnNodeDataWhenValidParametersArePassed()
        {
            var expectedNode = new Node
            {
                Id = "guid-1",
                Data = "{}",
                Type = "Person"
            };
            var expected = JsonConvert.SerializeObject(expectedNode);
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.GetFileContents(It.IsAny<string>()))
                .ReturnsAsync(expected);

            var result = await _controller.GetNode("some-db", "some-graph", "guid-1", someUserId);

            var actual = JsonConvert.SerializeObject(result);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void ShouldThrowExceptionWhenInvalidParametersArePassed()
        {
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(false);
            await Assert.ThrowsAsync<BubblesException>(
                () => _controller.GetNode("some-db", "some-graph", "guid-1", someUserId));
        }

        #endregion

        #region ConnectNodes

        [Fact]
        public async Task ShouldConnectNodesWhenValidNodeIdsArePassedAndCreateFolderForTypesAndRelationshipFiles()
        {
            var dbPath = "/some-folder/" + someUserId + "/some-db";
            const string leftNodeId = "guid-1";
            const string rightNodeId = "guid-2";
            const string leftNodeIdPath = "/some-folder/some-db/graphs/some-graph/" + leftNodeId + ".json";
            const string rightNodeIdPath = "/some-folder/some-db/graphs/some-graph-2/" + rightNodeId + ".json";
            const string relationshipFolderLocation = "/some-folder/some-db/relationships/types";
            const string relationshipTypeFileLocation = "some-folder/some-db/relationships/types/Is_Brother_of.json";

            var expectedRelationship = new Relationship
            {
                LeftNodeId = "guid-1",
                RightNodeId = "guid-2",
                Type = "Is_Brother_of"
            };

            _fileWrapper.Setup(fs => fs.IsExists(It.Is<string>(x => x == dbPath))).Returns(true);
            _fileWrapper.Setup(fs => fs.SearchFiles(It.IsAny<string>(),
                    It.Is<string>(x => x == leftNodeId + ".json")))
                .Returns(leftNodeIdPath);
            _fileWrapper.Setup(fs => fs.SearchFiles(It.IsAny<string>(),
                    It.Is<string>(x => x == rightNodeId + ".json")))
                .Returns(rightNodeIdPath);
            _fileWrapper.Setup(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.Is<string>(x => x == relationshipFolderLocation))).Returns(false);
            _fileWrapper.Setup(fs => fs.CreateFolder(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.Is<string>(x => x == relationshipTypeFileLocation))).Returns(false);

            var result =
                await _controller.ConnectNode("some-db", "guid-1", "guid-2", "Is_Brother_of", "{}", someUserId);

            Assert.Equal(expectedRelationship.LeftNodeId, result.LeftNodeId);
            Assert.Equal(expectedRelationship.RightNodeId, result.RightNodeId);
            Assert.Equal(expectedRelationship.Type, result.Type);

            _fileWrapper.Verify(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            _fileWrapper.Verify(fs => fs.CreateFolder(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ShouldConnectNodesWhenValidNodeIdsArePassedAndNotCreateFolderForTypesAndRelationshipFiles()
        {
            var dbPath = "/some-folder/" + someUserId + "/some-db";
            const string leftNodeId = "guid-1";
            const string rightNodeId = "guid-2";
            var leftNodeIdPath = "/some-path/some-db/" + someUserId + "/graphs/some-graph/" + leftNodeId + ".json";
            var rightNodeIdPath = "/some-path/some-db/" + someUserId + "/graphs/some-graph-2/" + rightNodeId + ".json";
            var relationshipTypesFolderLocation = "/some-folder/" + someUserId + "/some-db/relationships/types";
            var relationshipTypeFileLocation =
                "/some-folder/" + someUserId + "/some-db/relationships/types/Is_Brother_of.json";

            var expectedRelationship = new Relationship
            {
                LeftNodeId = "guid-1",
                RightNodeId = "guid-2",
                Type = "Is_Brother_of"
            };
            var relationshipType = new RelationshipType
            {
                RelationshipIds = new List<string> { "guid-3" }
            };

            _fileWrapper.Setup(fs => fs.IsExists(It.Is<string>(x => x == dbPath))).Returns(true);
            _fileWrapper.Setup(fs => fs.SearchFiles(It.IsAny<string>(),
                    It.Is<string>(x => x == leftNodeId + ".json")))
                .Returns(leftNodeIdPath);
            _fileWrapper.Setup(fs => fs.SearchFiles(It.IsAny<string>(),
                    It.Is<string>(x => x == rightNodeId + ".json")))
                .Returns(rightNodeIdPath);
            _fileWrapper.Setup(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.Is<string>(x => x == relationshipTypesFolderLocation)))
                .Returns(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.Is<string>(x => x == relationshipTypeFileLocation))).Returns(true);
            _fileWrapper.Setup(fs => fs.GetFileContents(It.IsAny<string>()))
                .ReturnsAsync(JsonConvert.SerializeObject(relationshipType));

            var result = await _controller.ConnectNode("some-db", "guid-1", "guid-2",
                "Is_Brother_of", "{}", someUserId);

            Assert.Equal(expectedRelationship.LeftNodeId, result.LeftNodeId);
            Assert.Equal(expectedRelationship.RightNodeId, result.RightNodeId);
            Assert.Equal(expectedRelationship.Type, result.Type);

            _fileWrapper.Verify(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            _fileWrapper.Verify(fs => fs.CreateFolder(It.IsAny<string>()), Times.Never);
            _fileWrapper.Verify(fs => fs.GetFileContents(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void ShouldReturnRelationshipDataWhenValidIdIsPassed()
        {
            const string relationshipId = "guid-1";
            var relationship = new Relationship
            {
                Id = relationshipId,
                Data = "{}",
                LeftNodeId = "guid-node-1",
                RightNodeId = "guid-right-1",
                Type = "Is_Brother_of"
            };
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.GetFileContents(It.IsAny<string>()))
                .ReturnsAsync(JsonConvert.SerializeObject(relationship));
            var result = await _controller.GetRelationship("some-db", relationshipId, someUserId);
            var expected = JsonConvert.SerializeObject(relationship);

            var actual = JsonConvert.SerializeObject(result);

            Assert.Equal(expected, actual);
            _fileWrapper.Verify(fs => fs.IsExists(It.IsAny<string>()), Times.Exactly(2));
            _fileWrapper.Verify(fs => fs.GetFileContents(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowErrorWhenRelationshipDoesNotExists()
        {
            const string dbLocation = "/some-folder/some-db";
            _fileWrapper.Setup(fs => fs.IsExists(It.Is<string>(x => x == dbLocation))).Returns(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(false);
            await Assert.ThrowsAsync<BubblesException>(() => _controller.GetRelationship("some-db",
                "guid-1", someUserId));
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