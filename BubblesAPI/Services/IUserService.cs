using System.Threading.Tasks;
using BubblesAPI.DTOs;

namespace BubblesAPI.Services
{
    public interface IUserService
    {
        public Task<int> RegisterUser(RegisterUserRequest request);
    }
}