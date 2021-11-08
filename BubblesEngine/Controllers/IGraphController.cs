using BubblesEngine.Models;

namespace BubblesEngine.Controllers
{
    public interface IGraphController
    {
        public bool CreateGraph(string graphName, string databaseName, string userId);
        public Graph GetGraph(string databaseName, string graphName, string userId);
    }
}