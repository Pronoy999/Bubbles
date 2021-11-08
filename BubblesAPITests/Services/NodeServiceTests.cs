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
        private readonly Mock<INodeController> _dbController;

        public NodeServiceTests()
        {
            _dbController = new Mock<INodeController>();
            _nodeService = new NodeService(_dbController.Object);
        }

        [Fact]
        public async Task ShouldCreateNodeWithValidData()
        {
            const string someNodeId = "type-guid-1";
            _dbController.Setup(db => db.CreateNode(It.IsAny<string>(), It.IsAny<string>(),
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
            _dbController.Setup(db => db.CreateNode(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
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

        [Fact]
        public async Task ShouldReturnNodeDataWhenNodeExists()
        {
            var expectedNode = new Node
            {
                Id = "some-id",
                Data = "{'hello':'world'}",
                Type = "some-type"
            };
            _dbController.Setup(db => db.GetNode(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(expectedNode);
            var request = new GetNodeRequest()
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
            _dbController.Setup(db => db.GetNode(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(exception);
            var request = new GetNodeRequest()
            {
                DbName = "some-db",
                GraphName = "some-graphName",
                NodeId = "some-invalid-id"
            };

            Assert.ThrowsAsync<BubblesException>(() => _nodeService.GetNode(request, "some-user-id"));
        }
    }
}