using System.Threading.Tasks;
using BubblesAPI.DTOs;
using BubblesEngine.Controllers;
using BubblesEngine.Models;

namespace BubblesAPI.Services.Implementation
{
    public class NodeService : INodeService
    {
        private readonly IDbController _dbController;

        public NodeService(IDbController dbController)
        {
            _dbController = dbController;
        }

        public async Task<string> CreateNode(CreateNodeRequest request, string userId)
        {
            return await _dbController.CreateNode(request.Database, request.Graph, request.Type, request.Data, userId);
        }

        public async Task<Node> GetNode(GetNodeRequest request, string userId)
        {
            return await _dbController.GetNode(request.DbName, request.GraphName, request.NodeId, userId);
        }
    }
}