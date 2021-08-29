using System.Collections.Generic;
using Newtonsoft.Json;

namespace BubblesEngine.Models
{
    public class Database
    {
        public string DatabaseName { get; set; }
        public List<Graph> Graphs { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}