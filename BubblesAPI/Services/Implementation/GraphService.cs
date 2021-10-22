using BubblesAPI.DTOs;
using BubblesEngine.Controllers;
using BubblesEngine.Models;

namespace BubblesAPI.Services.Implementation
{
    public class GraphService : IGraphService
    {
        private readonly IDbController _dbController;

        public GraphService(IDbController dbController)
        {
            _dbController = dbController;
        }

        public bool CreateGraph(CreateGraphRequest request, string userId)
        {
            return _dbController.CreateGraph(request.GraphName, request.DbName, userId);
        }

        public Graph GetGraph(GetGraphRequest request, string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}