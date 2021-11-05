using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BubblesAPI.Controllers;
using BubblesAPI.DTOs;
using BubblesAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BubblesAPITests.Controllers
{
    public class NodeControllerTests
    {
        private readonly Mock<INodeService> _nodeService;
        private readonly NodeController _nodeController;

        public NodeControllerTests()
        {
            _nodeService = new Mock<INodeService>();
            _nodeController = new NodeController(_nodeService.Object);
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
        public async Task ShouldReturn200WhenNodeIsCreatedWithValidData()
        {
            const string someNodeId = "type-guid-1";
            var request = new CreateNodeRequest
            {
                Data = "{ }",
                Database = "some-database",
                Graph = "some-graph",
                Type = "some-type"
            };
            _nodeService.Setup(node => node.CreateNode(request, It.IsAny<string>()))
                .ReturnsAsync(someNodeId);
            _nodeController.ControllerContext.HttpContext = GetUserContext();

            var result = await _nodeController.CreateNode(request);
            var actualResult = result as OkObjectResult;

            Assert.NotNull(actualResult);
            Assert.Equal((int)HttpStatusCode.OK, actualResult.StatusCode);
        }

        [Fact]
        public async Task ShouldReturn401ForUnAuthorizedAccess()
        {
            var request = new CreateNodeRequest
            {
                Data = "{ }",
                Database = "some-database",
                Graph = "some-graph",
                Type = "some-type"
            };
            var result = await _nodeController.CreateNode(request);
            var actualResponse = result as UnauthorizedResult;
            Assert.Equal((int)HttpStatusCode.Unauthorized, actualResponse.StatusCode);
        }
    }
}