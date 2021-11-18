using System.Threading.Tasks;
using BubblesAPI.DTOs;
using BubblesEngine.Controllers;
using BubblesEngine.Models;

namespace BubblesAPI.Services.Implementation
{
    public class NodeService : INodeService
    {
        private readonly INodeController _nodeController;

        public NodeService(INodeController nodeController)
        {
            _nodeController = nodeController;
        }

        public async Task<string> CreateNode(CreateNodeRequest request, string userId)
        {
            return await _nodeController.CreateNode(request.Database, request.Graph, request.Type, request.Data,
                userId);
        }

        public async Task<Node> GetNode(GetNodeRequest request, string userId)
        {
            return await _nodeController.GetNode(request.DbName, request.GraphName, request.NodeId, userId);
        }

        public async Task<Relationship> ConnectNode(ConnectNodeRequest request, string userId)
        {
            return await _nodeController.ConnectNode(request.DbName, request.LeftNodeId, request.RightNodeId,
                request.RelationshipType,
                request.Data, userId);
        }
    }
}