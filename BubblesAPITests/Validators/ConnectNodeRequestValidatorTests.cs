using BubblesAPI.DTOs;
using BubblesAPI.Validators;
using Xunit;

namespace BubblesAPITests.Validators
{
    public class ConnectNodeRequestValidatorTests
    {
        private readonly ConnectNodeRequestValidator _validator;

        public ConnectNodeRequestValidatorTests()
        {
            _validator = new ConnectNodeRequestValidator();
        }

        [Fact]
        public void ShouldValidateWithAllData()
        {
            var request = new ConnectNodeRequest
            {
                Data = "{}",
                DbName = "some-db",
                LeftNodeId = "some-id-1",
                RightNodeId = "some-id-2",
                RelationshipType = "isFriendOf"
            };
            var result = _validator.Validate(request);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ShouldNotValidateWithNoDbName()
        {
            var request = new ConnectNodeRequest
            {
                Data = "{}",
                LeftNodeId = "some-id-1",
                RightNodeId = "some-id-2",
                RelationshipType = "isFriendOf"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ShouldNotValidateWithNoLeftNodeId()
        {
            var request = new ConnectNodeRequest
            {
                Data = "{}",
                DbName = "some-db",
                RightNodeId = "some-id-2",
                RelationshipType = "isFriendOf"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ShouldNotValidateWithNoRightNode()
        {
            var request = new ConnectNodeRequest
            {
                Data = "{}",
                DbName = "some-db",
                LeftNodeId = "some-id-1",
                RelationshipType = "isFriendOf"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ShouldNotValidateWithRelationshipType()
        {
            var request = new ConnectNodeRequest
            {
                Data = "{}",
                DbName = "some-db",
                LeftNodeId = "some-id-1",
                RightNodeId = "some-id-2"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
        }
    }
}