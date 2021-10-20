using System.Linq;
using BubblesAPI.DTOs;
using BubblesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubblesAPI.Controllers
{
    [ApiController]
    [Route("/db")]
    public class DbController : ControllerBase
    {
        private readonly IDbService _dbService;

        public DbController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult CreateDb([FromBody] CreateDbRequest request)
        {
            var user = this.HttpContext.User;
            var userId = user.Claims.First(c => c.Type == "id").Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            _dbService.CreateDb(request.DbName, userId);
            return Ok();
        }
    }
}