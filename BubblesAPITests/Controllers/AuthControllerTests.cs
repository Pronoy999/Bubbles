using System.Net;
using BubblesAPI.Controllers;
using BubblesAPI.DTOs;
using BubblesAPI.Exceptions;
using BubblesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BubblesAPITests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authService;
        private AuthController _authController;

        public AuthControllerTests()
        {
            _authService = new Mock<IAuthService>();
            _authController = new AuthController(_authService.Object);
        }

        [Fact]
        public void ShouldReturn200WhenValidCredentialsPassed()
        {
            var expectedResponse = new LoginResponse
            {
                UserId = "some-id",
                Token = "some-token"
            };
            _authService.Setup(s => s.Login(It.IsAny<LoginRequest>())).Returns(expectedResponse);

            var result = _authController.Login(It.IsAny<LoginRequest>());
            var actualResult = result as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, actualResult.StatusCode);
        }

        [Fact]
        public void ShouldReturn404WhenInvalidCredentialsPassed()
        {
            _authService.Setup(s => s.Login(It.IsAny<LoginRequest>())).Throws<UserNotFoundException>();
            var result = _authController.Login(It.IsAny<LoginRequest>());

            var actualResult = result as NotFoundObjectResult;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.NotFound, actualResult.StatusCode);
        }
    }
}