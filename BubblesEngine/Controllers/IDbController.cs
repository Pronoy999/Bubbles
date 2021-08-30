using System.Threading.Tasks;
using BubblesEngine.Models;

namespace BubblesEngine.Controllers
{
    public interface IDbController
    {
        public bool CreateDatabase(string databaseName);
        public bool CreateGraph(string graphName, string databaseName);
        public Task<bool> CreateNode(string databaseName, string graphName, string type, dynamic data);
        public Database GetDatabase(string databaseName);
        public Graph GetGraph(string databaseName, string graphName);
        public Node GetNode(string nodeId);
    }
}