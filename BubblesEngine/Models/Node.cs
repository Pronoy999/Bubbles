namespace BubblesEngine.Models
{
    public class Node
    {
        public string Id { get; set; }
        public dynamic Data { get; set; }
        public string Type { get; set; }
        public Relationship[] Relationships { get; set; }
    }
}