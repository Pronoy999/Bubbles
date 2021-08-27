using System.Collections.Generic;

namespace BubblesEngine.Models
{
    public class Database
    {
        public string DatabaseName { get; set; }
        public List<Graph> Graphs { get; set; }
    }
}