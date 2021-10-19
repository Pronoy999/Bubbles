using BubblesAPI.DTOs;

namespace BubblesAPI.Services
{
    public interface IDbService
    {
        public bool CreateDb(string dbName);
        public DatabaseResponse GetDb(string dbName);
        public bool RemoveDb(string dbName);
    }
}