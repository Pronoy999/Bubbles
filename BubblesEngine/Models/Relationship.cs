using Newtonsoft.Json;

namespace BubblesEngine.Models
{
    public class Relationship
    {
        public string Id { get; set; }
        public string LeftNodeId { get; set; }
        public string RightNodeId { get; set; }
        public string Type { get; set; }
        public dynamic Data { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}