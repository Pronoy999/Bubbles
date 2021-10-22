using BubblesAPI.DTOs;
using BubblesAPI.Helpers;
using BubblesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BubblesAPI.Controllers
{
    [ApiController]
    [Route("/graph")]
    public class GraphController : ControllerBase
    {
        private readonly IGraphService _graphService;

        public GraphController(IGraphService graphService)
        {
            _graphService = graphService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult CreateGraph([FromBody] CreateGraphRequest request)
        {
            var userId = Utils.GetUserId(HttpContext?.User);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            var isCreated = _graphService.CreateGraph(request, userId);
            return Ok(new CreateGraphResponse() { GraphName = request.GraphName, IsCreated = isCreated });
        }
    }
}