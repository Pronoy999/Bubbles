using System.Threading.Tasks;
using BubblesEngine.Models;

namespace BubblesEngine.Controllers
{
    public interface IDbController
    {
        public bool CreateGraph(string graphName, string databaseName, string userId);
        public Graph GetGraph(string databaseName, string graphName, string userId);
        public Task<string> CreateNode(string databaseName, string graphName, string type, dynamic data, string userId);
        public Task<Node> GetNode(string database, string graphName, string nodeId, string userId);

        public Task<Relationship> ConnectNode(string databaseName, string leftNodeId, string rightNodeId,
            string relationshipType, dynamic data, string userId);

        public Task<Relationship> GetRelationship(string database, string relationshipId, string userId);
        public Task<Node> SearchNodeById(string databaseName, string nodeId, string userId);
        public Task<Node> SearchNodeByData(string databaseName, string data, string userId);
    }
}