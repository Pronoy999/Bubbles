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
    public class NodeControllerTests
    {
        private readonly Mock<IFileWrapper> _fileWrapper;
        private readonly NodeController _controller;
        private readonly string someUserId = "some-user-id";

        public NodeControllerTests()
        {
            _fileWrapper = new Mock<IFileWrapper>();
            _controller = new NodeController(_fileWrapper.Object);
            Environment.SetEnvironmentVariable(Constants.DbRootFolderKey, "/some-folder");
            Environment.SetEnvironmentVariable(Constants.TypesFolderName, "types");
        }

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
            const string someType = "Person";

            var path = Environment.GetEnvironmentVariable(Constants.DbRootFolderKey) + Path.DirectorySeparatorChar +
                       someUserId + Path.DirectorySeparatorChar + dbName + Path.DirectorySeparatorChar +
                       Constants.GraphFolderName + Path.DirectorySeparatorChar +
                       graphName;
            var typePath = path + Path.DirectorySeparatorChar + "types" + Path.DirectorySeparatorChar + someType;
            _fileWrapper.Setup(fs => fs.IsDirectoryExists(It.Is<string>(x => x == path))).Returns(true);
            _fileWrapper.Setup(fs => fs.IsDirectoryExists(It.Is<string>(x => x == typePath))).Returns(true);
            _fileWrapper.Setup(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.CreateFolder(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.GetFileContents(It.IsAny<string>())).ReturnsAsync(typesData);

            var result = await _controller.CreateNode(dbName, graphName, someType, nodeData, someUserId);

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

            _fileWrapper.Setup(fs => fs.IsDirectoryExists(It.Is<string>(x => x == dbPath))).Returns(true);
            _fileWrapper.Setup(fs => fs.SearchFiles(It.IsAny<string>(),
                    It.Is<string>(x => x == leftNodeId + ".json")))
                .Returns(leftNodeIdPath);
            _fileWrapper.Setup(fs => fs.SearchFiles(It.IsAny<string>(),
                    It.Is<string>(x => x == rightNodeId + ".json")))
                .Returns(rightNodeIdPath);
            _fileWrapper.Setup(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            _fileWrapper.Setup(fs => fs.IsDirectoryExists(It.Is<string>(x => x == relationshipFolderLocation)))
                .Returns(false);
            _fileWrapper.Setup(fs => fs.CreateFolder(It.IsAny<string>())).Returns(true);
            _fileWrapper.Setup(fs => fs.IsExists(It.Is<string>(x => x == relationshipTypeFileLocation))).Returns(false);

            var result =
                await _controller.ConnectNode("some-db", "guid-1", "guid-2", "Is_Brother_of", "{}", someUserId);

            Assert.Equal(expectedRelationship.LeftNodeId, result.LeftNodeId);
            Assert.Equal(expectedRelationship.RightNodeId, result.RightNodeId);
            Assert.Equal(expectedRelationship.Type, result.Type);

            _fileWrapper.Verify(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
            _fileWrapper.Verify(fs => fs.CreateFolder(It.IsAny<string>()), Times.Exactly(2));
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
            var relationshipFolderLocation = "/some-folder/" + someUserId + "/some-db/relationships";
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

            _fileWrapper.Setup(fs => fs.IsDirectoryExists(It.Is<string>(x => x == dbPath))).Returns(true);
            _fileWrapper.Setup(fs => fs.SearchFiles(It.IsAny<string>(),
                    It.Is<string>(x => x == leftNodeId + ".json")))
                .Returns(leftNodeIdPath);
            _fileWrapper.Setup(fs => fs.SearchFiles(It.IsAny<string>(),
                    It.Is<string>(x => x == rightNodeId + ".json")))
                .Returns(rightNodeIdPath);
            _fileWrapper.Setup(fs => fs.CreateFile(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            _fileWrapper.Setup(fs => fs.IsDirectoryExists(It.Is<string>(x => x == relationshipFolderLocation)))
                .Returns(true);
            _fileWrapper.Setup(fs => fs.IsDirectoryExists(It.Is<string>(x => x == relationshipTypesFolderLocation)))
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

        #endregion
    }
}