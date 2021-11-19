using System.Collections.Generic;
using System.Threading.Tasks;
using BubblesAPI.DTOs;
using BubblesEngine.Controllers;
using BubblesEngine.Models;

namespace BubblesAPI.Services.Implementation
{
    public class RelationshipService : IRelationshipService
    {
        private readonly IRelationshipController _relationshipController;

        public RelationshipService(IRelationshipController relationshipController)
        {
            _relationshipController = relationshipController;
        }

        public async Task<Relationship> GetRelationship(GetRelationshipRequest request, string userId)
        {
            return await _relationshipController.GetRelationship(request.DbName, request.RelationshipId, userId);
        }

        public List<string> GetAllRelationships(string databaseName, string userId)
        {
            return _relationshipController.GetAllRelationships(databaseName, userId);
        }
    }
}