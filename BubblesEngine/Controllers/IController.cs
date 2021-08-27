using System.Threading.Tasks;

namespace BubblesEngine.Controllers
{
    public interface IController
    {
        public Task<string> CreateGraph(string graphName);
        public Task<string> CreateDatabase(string databaseName);
        public Task<string> CreateNode(string databaseId, string graphId, string type, dynamic data);
    }
}