using Newtonsoft.Json;

namespace BubblesEngine.Models
{
    public class Node
    {
        public string Id { get; set; }
        public dynamic Data { get; set; }
        public string Type { get; set; }    
        public string[] RelationshipIds { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}