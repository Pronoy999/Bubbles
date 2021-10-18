using System.Threading.Tasks;
using BubblesAPI.DTOs;
using BubblesAPI.Exceptions;
using BubblesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubblesAPI.Controllers
{
    [ApiController]
    [Route("/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            try{
                var result = await _userService.RegisterUser(request);
                return Ok(result);
            }
            catch (UserAlreadyRegisterException e){
                return BadRequest(new BubblesApiException(e));
            }
        }
    }
}