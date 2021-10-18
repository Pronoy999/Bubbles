using System.Net;
using System.Threading.Tasks;
using BubblesAPI.DTOs;
using BubblesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BubblesAPITests.Controllers
{
    public class UserController
    {
        private readonly BubblesAPI.Controllers.UserController _userController;
        private readonly Mock<IUserService> _userService;

        public UserController()
        {
            _userService = new Mock<IUserService>();
            _userController = new BubblesAPI.Controllers.UserController(_userService.Object);
        }

        [Fact]
        public async Task ShouldReturn200WhenValidRequestIsPassed()
        {
            var registerUserResponse = new RegisterUserResponse
            {
                UserId = "some-user-id",
                Token = "some-token"
            };
            _userService.Setup(s => s.RegisterUser(It.IsAny<RegisterUserRequest>()))
                .ReturnsAsync(registerUserResponse);

            var result = await _userController.RegisterUser(It.IsAny<RegisterUserRequest>());
            
            var actualResult = result as OkObjectResult;
            
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, actualResult.StatusCode);
            _userService.Verify(r => r.RegisterUser(It.IsAny<RegisterUserRequest>()), Times.Once);
        }
    }
}