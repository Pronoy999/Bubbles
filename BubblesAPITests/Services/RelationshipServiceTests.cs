using System.Threading.Tasks;
using BubblesAPI.DTOs;
using BubblesAPI.Services;
using BubblesAPI.Services.Implementation;
using BubblesEngine.Controllers;
using BubblesEngine.Exceptions;
using BubblesEngine.Models;
using Moq;
using Xunit;

namespace BubblesAPITests.Services
{
    public class RelationshipServiceTests
    {
        private readonly IRelationshipService _relationshipService;
        private readonly Mock<IRelationshipController> _relationshipController;

        public RelationshipServiceTests()
        {
            _relationshipController = new Mock<IRelationshipController>();
            _relationshipService = new RelationshipService(_relationshipController.Object);
        }

        #region GetRelationship

        [Fact]
        public async Task ShouldGetRelationshipWithValidData()
        {
            var request = new GetRelationshipRequest
            {
                DbName = "some-db",
                RelationshipId = "some-rs-id"
            };
            var relationship = new Relationship
            {
                Id = "some-rs-id",
                Data = "{}",
                LeftNodeId = "some-id-1",
                RightNodeId = "some-id-2",
            };
            _relationshipController.Setup(node =>
                    node.GetRelationship(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(relationship);

            var result = await _relationshipService.GetRelationship(request, "some-user-id");

            Assert.NotNull(result);
            Assert.Equal(request.RelationshipId, result.Id);
        }

        [Fact]
        public void ShouldThrowExceptionWhenRelationshipNotExists()
        {
            var request = new GetRelationshipRequest
            {
                DbName = "some-db",
                RelationshipId = "some-rs-id"
            };
            var exception = new BubblesException(new RelationshipNotFoundException());
            _relationshipController.Setup(node =>
                    node.GetRelationship(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(exception);

            Assert.ThrowsAsync<BubblesException>(() => _relationshipService.GetRelationship(request, "some-user-id"));
        }

        #endregion
    }
}