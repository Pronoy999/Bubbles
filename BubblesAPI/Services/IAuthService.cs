using BubblesAPI.DTOs;

namespace BubblesAPI.Services
{
    public interface IAuthService
    {
        public LoginResponse Login(LoginRequest request);
    }
}