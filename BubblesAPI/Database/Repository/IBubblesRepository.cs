using System.Threading.Tasks;
using BubblesAPI.Database.Models;
using BubblesAPI.DTOs;

namespace BubblesAPI.Database.Repository
{
    public interface IBubblesRepository
    {
        Task<string> SaveUser(RegisterUserRequest request);
        Task<User> GetUserById(string userId);
        Task<User> GetUserByEmail(string emailId);
    }
}