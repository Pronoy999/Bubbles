using BubblesAPI.DTOs;
using BubblesAPI.Exceptions;
using BubblesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BubblesAPI.Controllers
{
    [ApiController]
    [Route("/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            try{
                var result = _authService.Login(request);
                return Ok(result);
            }
            catch (UserNotFoundException){
                return NotFound(new BubblesApiException(new UserNotFoundException()));
            }
        }
    }
}