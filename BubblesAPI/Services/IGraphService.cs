using BubblesAPI.DTOs;
using BubblesEngine.Models;

namespace BubblesAPI.Services
{
    public interface IGraphService
    {
        public bool CreateGraph(CreateGraphRequest request, string userId);
        public Graph GetGraph(string dbName, string graphName, string userId);
    }
}