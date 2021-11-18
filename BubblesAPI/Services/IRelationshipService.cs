using System.Threading.Tasks;
using BubblesAPI.DTOs;
using BubblesEngine.Models;

namespace BubblesAPI.Services
{
    public interface IRelationshipService
    {
        public Task<Relationship> GetRelationship(GetRelationshipRequest request, string userId);
    }
}