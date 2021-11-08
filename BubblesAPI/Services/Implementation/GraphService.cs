using BubblesAPI.DTOs;
using BubblesEngine.Controllers;
using BubblesEngine.Models;

namespace BubblesAPI.Services.Implementation
{
    public class GraphService : IGraphService
    {
        private readonly IGraphController _graphController;

        public GraphService(IGraphController graphController)
        {
            _graphController = graphController;
        }

        public bool CreateGraph(CreateGraphRequest request, string userId)
        {
            return _graphController.CreateGraph(request.GraphName, request.DbName, userId);
        }

        public Graph GetGraph(string dbName, string graphName, string userId)
        {
            return _graphController.GetGraph(dbName, graphName, userId);
        }
    }
}