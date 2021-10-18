using System.Collections.Generic;
using AutoMapper;
using BubblesAPI.Authentication;
using BubblesAPI.Database.Models;
using BubblesAPI.Database.Repository;
using BubblesAPI.DTOs;
using BubblesAPI.Exceptions;
using BubblesAPI.Services;
using BubblesAPI.Services.Implementation;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace BubblesAPITests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IAuthentication> _authentication;
        private readonly Mock<IBubblesRepository> _bubblesRepository;
        private readonly IAuthService _authService;
        private readonly Mock<IMapper> _mapper;

        public AuthServiceTests()
        {
            _authentication = new Mock<IAuthentication>();
            _bubblesRepository = new Mock<IBubblesRepository>();
            _mapper = new Mock<IMapper>();
            var inMemoryValues = new Dictionary<string, string>
            {
                { "Jwt:key", "some-key" },
                { "Jwt:Issuer", "some-issuer" }
            };
            IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemoryValues).Build();
            _authService = new AuthService(_bubblesRepository.Object, _authentication.Object, configuration,
                _mapper.Object);
        }

        [Fact]
        public void ShouldValidateAUserWithValidCredentials()
        {
            var expectedUser = new User
            {
                UserId = "some-id",
                Email = "some-email@example.com",
                FirstName = "Test First Name",
                LastName = "Test Last Name"
            };
            const string someToken = "some-token";

            _authentication.Setup(a => a.GetToken(It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(someToken);
            _bubblesRepository.Setup(r => r.ValidateCredentials(It.IsAny<LoginRequest>()))
                .Returns(expectedUser);

            var result = _authService.Login(It.IsAny<LoginRequest>());

            Assert.NotNull(result);
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(someToken, result.Token);
        }

        [Fact]
        public void ShouldThrowErrorIfUserNotFound()
        {
            _bubblesRepository.Setup(r => r.ValidateCredentials(It.IsAny<LoginRequest>()))
                .Throws<UserNotFoundException>();

            Assert.Throws<UserNotFoundException>(() => _authService.Login(It.IsAny<LoginRequest>()));
        }
    }
}