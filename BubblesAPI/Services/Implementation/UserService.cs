using System.Threading.Tasks;
using BubblesAPI.Authentication;
using BubblesAPI.Database.Repository;
using BubblesAPI.DTOs;
using Microsoft.Extensions.Configuration;

namespace BubblesAPI.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IBubblesRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly IAuthentication _authentication;

        public UserService(IBubblesRepository repository, IAuthentication authentication, IConfiguration configuration)
        {
            _repository = repository;
            _authentication = authentication;
            _configuration = configuration;
        }

        public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request)
        {
            var userId = await _repository.SaveUser(request);
            return new RegisterUserResponse
            {
                UserId = userId,
                Token = _authentication.GetToken(userId, _configuration.GetSection("Jwt").GetValue<string>("Key"),
                    _configuration.GetSection("Jwt").GetValue<string>("Issuer"))
            };
        }
    }
}