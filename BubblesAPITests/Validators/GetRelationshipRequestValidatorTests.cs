using BubblesAPI.DTOs;
using BubblesAPI.Validators;
using Xunit;

namespace BubblesAPITests.Validators
{
    public class GetRelationshipRequestValidatorTests
    {
        private readonly GetRelationshipRequestValidator _validator;

        public GetRelationshipRequestValidatorTests()
        {
            _validator = new GetRelationshipRequestValidator();
        }

        [Fact]
        public void ShouldValidateWithAllData()
        {
            var request = new GetRelationshipRequest()
            {
                DbName = "some-db",
                RelationshipId = "some-rs-id"
            };
            var result = _validator.Validate(request);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ShouldNotValidateWithNoDbName()
        {
            var request = new GetRelationshipRequest()
            {
                RelationshipId = "some-rs-id"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ShouldNotValidateWithNoRelationshipId()
        {
            var request = new GetRelationshipRequest()
            {
                DbName = "some-db"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
        }
    }
}