using System.Collections.Generic;
using System.Threading.Tasks;
using BubblesAPI.Authentication;
using BubblesAPI.Database.Repository;
using BubblesAPI.DTOs;
using BubblesAPI.Exceptions;
using BubblesAPI.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace BubblesAPITests.Services
{
    public class UserService
    {
        private readonly Mock<IAuthentication> _authentication;
        private readonly Mock<IBubblesRepository> _bubblesRepository;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public UserService()
        {
            _authentication = new Mock<IAuthentication>();
            _bubblesRepository = new Mock<IBubblesRepository>();
            var inMemoryValues = new Dictionary<string, string>
            {
                { "Jwt:key", "some-key" },
                { "Jwt:Issuer", "some-issuer" }
            };
            _configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemoryValues).Build();
            _userService = new BubblesAPI.Services.Implementation.UserService(_bubblesRepository.Object,
                _authentication.Object, _configuration);
        }

        [Fact]
        public async Task ShouldRegisterAValidUser()
        {
            const string expectedUserId = "user-id-1";
            const string expectedToken = "user-token";

            _bubblesRepository.Setup(r => r.SaveUser(It.IsAny<RegisterUserRequest>()))
                .ReturnsAsync(expectedUserId);
            _bubblesRepository.Setup(r => r.GetUserByEmail(It.IsAny<string>())).Throws<UserNotFoundException>();
            _authentication.Setup(a => a.GetToken(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(expectedToken);

            var result = await _userService.RegisterUser(It.IsAny<RegisterUserRequest>());

            Assert.NotNull(result);
            Assert.Equal(expectedToken, result.Token);
            Assert.Equal(expectedUserId, result.UserId);
            _bubblesRepository.Verify(r => r.SaveUser(It.IsAny<RegisterUserRequest>()), Times.Once);
            _authentication.Verify(a => a.GetToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task ShouldNotRegisterARegisteredUser()
        {
            _bubblesRepository.Setup(r => r.SaveUser(It.IsAny<RegisterUserRequest>()))
                .Throws<UserAlreadyRegisterException>();
            await Assert.ThrowsAsync<UserAlreadyRegisterException>(() => _userService.RegisterUser(It.IsAny<RegisterUserRequest>()));
        }
    }
}