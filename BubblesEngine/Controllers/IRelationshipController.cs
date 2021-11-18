using System.Collections.Generic;
using System.Threading.Tasks;
using BubblesEngine.Models;

namespace BubblesEngine.Controllers
{
    public interface IRelationshipController
    {
        public Task<Relationship> GetRelationship(string database, string relationshipId, string userId);
        public List<string> GetAllRelationships(string database, string userId);
    }
}