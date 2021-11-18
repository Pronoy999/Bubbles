using BubblesAPI.DTOs;
using BubblesAPI.Validators;
using Xunit;

namespace BubblesAPITests.Validators
{
    public class CreateNodeRequestValidatorTests
    {
        private readonly CreateNodeRequestValidator _validator;

        public CreateNodeRequestValidatorTests()
        {
            _validator = new CreateNodeRequestValidator();
        }

        [Fact]
        public void ShouldValidateWithAllData()
        {
            var request = new CreateNodeRequest
            {
                Data = "{ }",
                Database = "some-database",
                Graph = "some-graph",
                Type = "some-type"
            };
            var result = _validator.Validate(request);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ShouldNotValidateWhenDatabaseIsMissing()
        {
            var request = new CreateNodeRequest
            {
                Graph = "some-graph",
                Type = "some-type"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ShouldNotValidateWhenGraphIsMissing()
        {
            var request = new CreateNodeRequest
            {
                Database = "some-database",
                Type = "some-type"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
        }
        [Fact]
        public void ShouldNotValidateWhenTypeIsMissing()
        {
            var request = new CreateNodeRequest
            {
                Data = "{ }",
                Database = "some-database",
                Graph = "some-graph",
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
        }
    }
}