using BubblesAPI.DTOs;
using BubblesAPI.Services;
using BubblesEngine.Controllers;
using BubblesEngine.Exceptions;
using BubblesEngine.Models;
using Moq;
using Xunit;

namespace BubblesAPITests.Services
{
    public class GraphService
    {
        private readonly Mock<IDbController> _dbController;
        private readonly IGraphService _graphService;

        public GraphService()
        {
            _dbController = new Mock<IDbController>();
            _graphService = new BubblesAPI.Services.Implementation.GraphService(_dbController.Object);
        }

        [Fact]
        public void ShouldCreateAGraphWhenValidDataIsPassed()
        {
            _dbController.Setup(db => db.CreateGraph(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            var request = new CreateGraphRequest
            {
                DbName = "someDb",
                GraphName = "someGraph"
            };
            const string someUserId = "some-user-id";

            var result = _graphService.CreateGraph(request, someUserId);
            Assert.True(result);
        }

        [Fact]
        public void ShouldGetAGraphWhenValidParamsPassed()
        {
            var expectedGraph = new Graph()
            {
                GraphName = "some-graph"
            };
            _dbController.Setup(db => db.GetGraph(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(expectedGraph);
            var result = _graphService.GetGraph("some-db", "some-graph", "some-user-id");
            Assert.NotNull(result);
            Assert.Equal(expectedGraph.GraphName, result.GraphName);
        }

        [Fact]
        public void ShouldThrowExceptionWhenGraphNotFound()
        {
            var exception = new BubblesException(new GraphNotFoundException());
            _dbController.Setup(db => db.GetGraph(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(exception);
            Assert.Throws<BubblesException>(() => _graphService.GetGraph("some-db", "some-graph", "some-user"));
        }
    }
}