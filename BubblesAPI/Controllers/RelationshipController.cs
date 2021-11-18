using System;
using System.Threading.Tasks;
using BubblesAPI.DTOs;
using BubblesAPI.Helpers;
using BubblesAPI.Services;
using BubblesEngine.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubblesAPI.Controllers
{
    [ApiController]
    [Route("/relationship")]
    public class RelationshipController : ControllerBase
    {
        private readonly IRelationshipService _relationshipService;

        public RelationshipController(IRelationshipService relationshipService)
        {
            _relationshipService = relationshipService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetRelationship([FromQuery] GetRelationshipRequest request)
        {
            var userId = Utils.GetUserId(HttpContext?.User);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            try{
                var result = await _relationshipService.GetRelationship(request, userId);
                return Ok(result);
            }
            catch (BubblesException e){
                Exception exception = e.InnerException switch
                {
                    DatabaseNotFoundException => new DatabaseNotFoundException(),
                    RelationshipNotFoundException => new RelationshipNotFoundException(),
                    _ => null
                };
                return exception != null ? NotFound(exception) : Problem();
            }
        }
    }
}