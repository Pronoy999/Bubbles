using BubblesAPI.DTOs;
using BubblesAPI.Services;
using BubblesEngine.Controllers;
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
    }
}