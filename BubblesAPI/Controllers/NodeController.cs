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
    [Route("/node")]
    public class NodeController : ControllerBase
    {
        private readonly INodeService _nodeService;

        public NodeController(INodeService nodeService)
        {
            _nodeService = nodeService;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateNode([FromBody] CreateNodeRequest request)
        {
            var userId = Utils.GetUserId(HttpContext?.User);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            try{
                var nodeId = await _nodeService.CreateNode(request, userId);
                return Ok(new CreateNodeResponse() { NodeId = nodeId, IsCreated = true });
            }
            catch (BubblesException e){
                Exception exception = e.InnerException switch
                {
                    DatabaseNotFoundException => new DatabaseNotFoundException(),
                    GraphNotFoundException => new GraphNotFoundException(),
                    _ => null
                };
                return exception != null ? NotFound(exception) : Problem();
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetNode([FromQuery] GetNodeRequest request)
        {
            var userId = Utils.GetUserId(HttpContext?.User);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            try{
                var result = await _nodeService.GetNode(request, userId);
                return Ok(result);
            }
            catch (BubblesException e){
                Exception exception = e.InnerException switch
                {
                    DatabaseNotFoundException => new DatabaseNotFoundException(),
                    GraphNotFoundException => new GraphNotFoundException(),
                    NodeNotFoundException => new NodeNotFoundException(),
                    _ => null
                };
                return exception != null ? NotFound(exception) : Problem();
            }
        }
    }
}