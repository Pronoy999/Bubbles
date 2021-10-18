using BubblesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BubblesAPI.Controllers
{
    [ApiController]
    [Route("/user")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
    }
}