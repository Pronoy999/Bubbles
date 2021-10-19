using System.Collections.Generic;
using BubblesEngine.Models;

namespace BubblesAPI.DTOs
{
    public class DatabaseResponse
    {
        public string DatabaseName { get; set; }
        public List<Graph> Graphs { get; set; }
    }
}