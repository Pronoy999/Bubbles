using System.Threading.Tasks;
using BubblesAPI.DTOs;
using BubblesEngine.Models;

namespace BubblesAPI.Services
{
    public interface INodeService
    {
        public Task<string> CreateNode(CreateNodeRequest request, string userId);
        public Task<Node> GetNode(GetNodeRequest request, string userId);
        public Task<Relationship> ConnectNode(ConnectNodeRequest request, string userId);
        public Task<Relationship> GetRelationship(GetRelationshipRequest request, string userId);
    }
}