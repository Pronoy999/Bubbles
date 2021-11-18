using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BubblesAPI.Controllers;
using BubblesAPI.DTOs;
using BubblesAPI.Services;
using BubblesEngine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BubblesAPITests.Controllers
{
    public class RelationshipControllerTests
    {
        private readonly RelationshipController _relationshipController;
        private readonly Mock<IRelationshipService> _relationshipService;

        public RelationshipControllerTests()
        {
            _relationshipService = new Mock<IRelationshipService>();
            _relationshipController = new RelationshipController(_relationshipService.Object);
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

        #region GetRelationship

        [Fact]
        public async Task ShouldReturn200WhenValidRequestIsMade()
        {
            var request = new GetRelationshipRequest
            {
                DbName = "some-db",
                RelationshipId = "some-rs-id"
            };
            var relationship = new Relationship
            {
                Id = "some-rs-id",
                Data = "{}",
                LeftNodeId = "some-id-1",
                RightNodeId = "some-id-2",
            };
            _relationshipService.Setup(node => node.GetRelationship(It.IsAny<GetRelationshipRequest>(), It.IsAny<string>()))
                .ReturnsAsync(relationship);
            _relationshipController.ControllerContext.HttpContext = GetUserContext();

            var result = await _relationshipController.GetRelationship(request);

            var actualResult = result as OkObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, actualResult.StatusCode);
        }

        [Fact]
        public async Task ShouldReturn401WhenUnauthorizedAccessIsMade()
        {
            var request = new GetRelationshipRequest
            {
                DbName = "some-db",
                RelationshipId = "some-rs-id"
            };
            var result = await _relationshipController.GetRelationship(request);

            var actualResult = result as UnauthorizedResult;
            Assert.Equal((int)HttpStatusCode.Unauthorized, actualResult.StatusCode);
        }

        #endregion
    }
}