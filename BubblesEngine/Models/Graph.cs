using System.Collections.Generic;
using Newtonsoft.Json;

namespace BubblesEngine.Models
{
    public class Graph
    {
        public string GraphName { get; set; }
        public List<Node> Nodes { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}