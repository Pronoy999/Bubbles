using System.Net;
using System.Security.Claims;
using BubblesAPI.Controllers;
using BubblesAPI.DTOs;
using BubblesAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BubblesAPITests.Controllers
{
    public class DbControllerTests
    {
        private readonly Mock<IDbService> _dbService;
        private readonly DbController _controller;

        public DbControllerTests()
        {
            _dbService = new Mock<IDbService>();
            _controller = new DbController(_dbService.Object);
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
        public void ShouldReturn200WhenValidDbIsCreated()
        {
            _dbService.Setup(db => db.CreateDb(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _controller.ControllerContext.HttpContext = GetUserContext();
            var request = new CreateDbRequest
            {
                DbName = "some-db"
            };
            var response = _controller.CreateDb(request);
            var actualResponse = response as OkObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, actualResponse.StatusCode);
        }

        [Fact]
        public void ShouldReturn401WhenInValidCredentials()
        {
            var request = new CreateDbRequest
            {
                DbName = "some-db"
            };
            var response = _controller.CreateDb(request);
            var actualResponse = response as UnauthorizedResult;
            Assert.Equal((int)HttpStatusCode.Unauthorized, actualResponse.StatusCode);
        }

        [Fact]
        public void ShouldReturn200WhenSearchedForDbWithValidCredentials()
        {
            const string someDbName = "some-db-name";
            var expectedResponse = new DatabaseResponse()
            {
                DatabaseName = someDbName
            };
            _dbService.Setup(db => db.GetDb(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedResponse);
            _controller.ControllerContext.HttpContext = GetUserContext();

            var response = _controller.GetDb(someDbName);
            var actualResponse = response as OkObjectResult;
            Assert.Equal((int)HttpStatusCode.OK, actualResponse.StatusCode);
        }

        [Fact]
        public void ShouldReturn401WhenSearchedForDbWithInValidCredentials()
        {
            const string someDbName = "some-db-name";

            var response = _controller.GetDb(someDbName);
            var actualResponse = response as UnauthorizedResult;
            Assert.Equal((int)HttpStatusCode.Unauthorized, actualResponse.StatusCode);
        }
    }
}