using System.Collections.Generic;
using Newtonsoft.Json;

namespace BubblesEngine.Models
{
    public class RelationshipType
    {
        public List<string> RelationshipIds { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}