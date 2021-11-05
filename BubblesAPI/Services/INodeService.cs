using System.Threading.Tasks;
using BubblesAPI.DTOs;
using BubblesEngine.Models;

namespace BubblesAPI.Services
{
    public interface INodeService
    {
        public Task<string> CreateNode(CreateNodeRequest request, string userId);
        public Task<Node> GetNode(string database, string graph, string userId);
    }
}