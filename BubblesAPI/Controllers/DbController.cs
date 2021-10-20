using BubblesAPI.DTOs;
using BubblesAPI.Helpers;
using BubblesAPI.Services;
using BubblesEngine.Exceptions;
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
            var userId = Utils.GetUserId(HttpContext.User);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            _dbService.CreateDb(request.DbName, userId);
            return Ok(new CreateDbResponse
            {
                DbName = request.DbName, IsCreated = true
            });
        }

        [HttpGet("{dbName}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetDb([FromRoute] string dbName)
        {
            var userId = Utils.GetUserId(HttpContext.User);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            try{
                var response = _dbService.GetDb(dbName, userId);
                return Ok(response);
            }
            catch (BubblesException e){
                return e.InnerException is DatabaseNotFoundException ? BadRequest(new DatabaseNotFoundException()) : Problem();
            }
        }
    }
}