using AutoMapper;
using BubblesAPI.DTOs;
using BubblesEngine.Controllers;

namespace BubblesAPI.Services.Implementation
{
    public class DbService : IDbService
    {
        private readonly IDatabaseController _dbController;
        private readonly IMapper _mapper;

        public DbService(IDatabaseController dbController, IMapper mapper)
        {
            _dbController = dbController;
            _mapper = mapper;
        }

        public bool CreateDb(string dbName, string userId)
        {
            return _dbController.CreateDatabase(dbName, userId);
        }

        public DatabaseResponse GetDb(string dbName, string userId)
        {
            var response = _dbController.GetDatabase(dbName, userId);
            return _mapper.Map<DatabaseResponse>(response);
        }

        public bool RemoveDb(string dbName, string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}