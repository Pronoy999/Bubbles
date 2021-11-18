namespace BubblesAPI.DTOs
{
    public class ConnectNodeRequest
    {
        public string DbName { get; set; }
        public string LeftNodeId { get; set; }
        public string RightNodeId { get; set; }
        public string RelationshipType { get; set; }
        public string Data { get; set; }
    }
}