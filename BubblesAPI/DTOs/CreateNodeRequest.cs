namespace BubblesAPI.DTOs
{
    public class CreateNodeRequest
    {
        public string Database { get; set; }
        public string Graph { get; set; }
        public string Type { get; set; }
        public dynamic Data { get; set; }
    }
}