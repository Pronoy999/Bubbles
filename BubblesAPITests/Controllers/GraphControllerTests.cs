using System.Net;
using System.Security.Claims;
using BubblesAPI.Controllers;
using BubblesAPI.DTOs;
using BubblesAPI.Services;
using BubblesEngine.Exceptions;
using BubblesEngine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BubblesAPITests.Controllers
{
    public class GraphControllerTests
    {
        private readonly Mock<IGraphService> _graphService;
        private readonly GraphController _graphController;

        public GraphControllerTests()
        {
            _graphService = new Mock<IGraphService>();
            _graphController = new GraphController(_graphService.Object);
        }

        private static HttpContext GetUserContext()
        {
            var claims = new[]
            {
                new Claim("id", "some-user-id")
            };
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
            return new DefaultHttpContext { User = user };
        }

        [Fact]
        public void ShouldReturn200WhenValidRequestIsMade()
        {
            _graphService.Setup(g => g.CreateGraph(It.IsAny<CreateGraphRequest>(), It.IsAny<string>()))
                .Returns(true);
            _graphController.ControllerContext.HttpContext = GetUserContext();
            var request = new CreateGraphRequest()
            {
                DbName = "some-DbName",
                GraphName = "some-graph-name"
            };

            var result = _graphController.CreateGraph(request);
            var actualResponse = result as OkObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, actualResponse.StatusCode);
        }

        [Fact]
        public void ShouldReturn401WhenNoCredentialsIsPassed()
        {
            var request = new CreateGraphRequest()
            {
                DbName = "some-DbName",
                GraphName = "some-graph-name"
            };

            var result = _graphController.CreateGraph(request);
            var actualResponse = result as UnauthorizedResult;
            Assert.Equal((int)HttpStatusCode.Unauthorized, actualResponse.StatusCode);
        }

        [Fact]
        public void ShouldReturn200WhenGraphIsPresent()
        {
            var expectedGraph = new Graph()
            {
                GraphName = "friends",
            };
            _graphController.ControllerContext.HttpContext = GetUserContext();
            _graphService.Setup(g => g.GetGraph(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(expectedGraph);
            var result = _graphController.GetGraphs("test-db", "friends");
            var actualResponse = result as OkObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, actualResponse.StatusCode);
        }

        [Fact]
        public void ShouldReturn404WhenGraphIsNotPresent()
        {
            _graphService.Setup(g => g.GetGraph(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>()))
                .Throws(new BubblesException(new GraphNotFoundException()));
            _graphController.ControllerContext.HttpContext = GetUserContext();
            var result = _graphController.GetGraphs("some-db", "someGraph");
            var actualResponse = result as NotFoundObjectResult;
            Assert.Equal((int)HttpStatusCode.NotFound, actualResponse.StatusCode);
        }
    }
}