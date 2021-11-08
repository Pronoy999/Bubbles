using System.Threading.Tasks;
using BubblesEngine.Models;

namespace BubblesEngine.Controllers
{
    public interface IDbController
    {
       

        
        public Task<Node> SearchNodeById(string databaseName, string nodeId, string userId);
        public Task<Node> SearchNodeByData(string databaseName, string data, string userId);
    }
}