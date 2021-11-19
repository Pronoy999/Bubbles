using System.Collections.Generic;
using System.Threading.Tasks;
using BubblesAPI.DTOs;
using BubblesEngine.Models;

namespace BubblesAPI.Services
{
    public interface IRelationshipService
    {
        public Task<Relationship> GetRelationship(GetRelationshipRequest request, string userId);
        public List<string> GetAllRelationships(string databaseName, string userId);
    }
}