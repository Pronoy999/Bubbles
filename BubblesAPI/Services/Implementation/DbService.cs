using BubblesAPI.DTOs;
using BubblesEngine.Controllers;

namespace BubblesAPI.Services.Implementation
{
    public class DbService : IDbService
    {
        private readonly IDbController _dbController;

        public DbService(IDbController dbController)
        {
            _dbController = dbController;
        }

        public bool CreateDb(string dbName, string userId)
        {
            return _dbController.CreateDatabase(dbName, userId);
        }

        public DatabaseResponse GetDb(string dbName, string userId)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveDb(string dbName, string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}