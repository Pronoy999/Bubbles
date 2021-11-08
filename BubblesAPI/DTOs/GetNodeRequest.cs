using Microsoft.AspNetCore.Mvc;

namespace BubblesAPI.DTOs
{
    public class GetNodeRequest
    {
        [FromQuery] public string DbName { get; set; }
        [FromQuery] public string GraphName { get; set; }
        [FromQuery] public string NodeId { get; set; }
    }
}