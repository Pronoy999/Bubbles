using System.Threading.Tasks;
using BubblesAPI.DTOs;
using BubblesAPI.Services;
using BubblesAPI.Services.Implementation;
using BubblesEngine.Controllers;
using BubblesEngine.Exceptions;
using BubblesEngine.Models;
using Moq;
using Xunit;

namespace BubblesAPITests.Services
{
    public class NodeServiceTests
    {
        private readonly INodeService _nodeService;
        private readonly Mock<INodeController> _nodeController;

        public NodeServiceTests()
        {
            _nodeController = new Mock<INodeController>();
            _nodeService = new NodeService(_nodeController.Object);
        }

        #region CreateNode

        [Fact]
        public async Task ShouldCreateNodeWithValidData()
        {
            const string someNodeId = "type-guid-1";
            _nodeController.Setup(db => db.CreateNode(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(someNodeId);
            var request = new CreateNodeRequest
            {
                Data = "{ }",
                Database = "some-database",
                Graph = "some-graph",
                Type = "some-type"
            };
            var result = await _nodeService.CreateNode(request, "some-user-id");
            Assert.NotNull(result);
            Assert.Equal(someNodeId, result);
        }

        [Fact]
        public async Task ShouldNotCreateANodeWhenGraphDoesNotExists()
        {
            _nodeController.Setup(db => db.CreateNode(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new BubblesException(new GraphNotFoundException()));
            var request = new CreateNodeRequest
            {
                Data = "{ }",
                Database = "some-database",
                Graph = "some-graph",
                Type = "some-type"
            };
            await Assert.ThrowsAsync<BubblesException>(() => _nodeService.CreateNode(request, "some-user-id"));
        }

        #endregion

        #region GetNode

        [Fact]
        public async Task ShouldReturnNodeDataWhenNodeExists()
        {
            var expectedNode = new Node
            {
                Id = "some-id",
                Data = "{'hello':'world'}",
                Type = "some-type"
            };
            _nodeController.Setup(db => db.GetNode(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(expectedNode);
            var request = new GetNodeRequest
            {
                DbName = "some-db",
                GraphName = "some-graphName",
                NodeId = "some-id"
            };

            var result = await _nodeService.GetNode(request, "some-user-id");

            Assert.NotNull(result);
            Assert.Equal(expectedNode.Id, result.Id);
            Assert.Equal(expectedNode.Data, result.Data);
            Assert.Equal(expectedNode.Type, result.Type);
        }

        [Fact]
        public void ShouldThrowExceptionWhenInvalidNodeIdIsPassed()
        {
            var exception = new BubblesException(new NodeNotFoundException());
            _nodeController.Setup(db => db.GetNode(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(exception);
            var request = new GetNodeRequest
            {
                DbName = "some-db",
                GraphName = "some-graphName",
                NodeId = "some-invalid-id"
            };

            Assert.ThrowsAsync<BubblesException>(() => _nodeService.GetNode(request, "some-user-id"));
        }

        #endregion

        #region ConnectNode

        [Fact]
        public async Task ShouldConnectNodeWhenValidRequestIsPassed()
        {
            var request = new ConnectNodeRequest
            {
                Data = "{}",
                DbName = "some-db",
                LeftNodeId = "some-id-1",
                RightNodeId = "some-id-2",
                RelationshipType = "isFriendOf"
            };
            var relationship = new Relationship
            {
                Id = "some-rs-id",
                Data = "{}",
                LeftNodeId = "some-id-1",
                RightNodeId = "some-id-2",
            };
            _nodeController.Setup(node => node.ConnectNode(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(relationship);

            var result = await _nodeService.ConnectNode(request, "some-user-id");

            Assert.NotNull(result.Id);
            Assert.Equal(request.LeftNodeId, result.LeftNodeId);
        }

        [Fact]
        public void ShouldThrowExceptionWhenNodeDoesNotExists()
        {
            var request = new ConnectNodeRequest
            {
                Data = "{}",
                DbName = "some-db",
                LeftNodeId = "invalid-id",
                RightNodeId = "some-id-2",
                RelationshipType = "isFriendOf"
            };
            var exception = new BubblesException(new NodeNotFoundException());
            _nodeController.Setup(node => node.ConnectNode(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(exception);

            Assert.ThrowsAsync<BubblesException>(() => _nodeService.ConnectNode(request, "some-user-id"));
        }

        #endregion
    }
}