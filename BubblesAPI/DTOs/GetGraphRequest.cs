using Microsoft.AspNetCore.Mvc;

namespace BubblesAPI.DTOs
{
    public class GetGraphRequest
    {
        [FromRoute] public string DbName { get; set; }
        [FromQuery] public string GraphName { get; set; }
    }
}