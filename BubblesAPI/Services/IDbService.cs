using BubblesAPI.DTOs;

namespace BubblesAPI.Services
{
    public interface IDbService
    {
        public bool CreateDb(string dbName, string userId);
        public DatabaseResponse GetDb(string dbName, string userId);
        public bool RemoveDb(string dbName, string userId);
    }
}