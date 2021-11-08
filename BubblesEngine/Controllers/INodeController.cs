using System.Threading.Tasks;
using BubblesEngine.Models;

namespace BubblesEngine.Controllers
{
    public interface INodeController
    {
        public Task<string> CreateNode(string databaseName, string graphName, string type, dynamic data, string userId);
        public Task<Node> GetNode(string database, string graphName, string nodeId, string userId);

        public Task<Relationship> ConnectNode(string databaseName, string leftNodeId, string rightNodeId,
            string relationshipType, dynamic data, string userId);

        public Task<Relationship> GetRelationship(string database, string relationshipId, string userId);
    }
}