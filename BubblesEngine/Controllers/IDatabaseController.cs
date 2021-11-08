using BubblesEngine.Models;

namespace BubblesEngine.Controllers
{
    public interface IDatabaseController
    {
        public bool CreateDatabase(string databaseName, string userId);
        public Database GetDatabase(string databaseName, string userId);
    }
}