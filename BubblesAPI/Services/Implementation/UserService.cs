using System.Threading.Tasks;
using BubblesAPI.Database.Repository;
using BubblesAPI.DTOs;

namespace BubblesAPI.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IBubblesRepository _repository;

        public UserService(IBubblesRepository repository)
        {
            _repository = repository;
        }

        public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request)
        {
            var userId = await _repository.SaveUser(request);
            return new RegisterUserResponse
            {
                UserId = userId,
                Token = ""//TODO:Create JWT.
            };
        }
    }
}