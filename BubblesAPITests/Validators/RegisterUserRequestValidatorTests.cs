using BubblesAPI.DTOs;
using BubblesAPI.Validators;
using Xunit;

namespace BubblesAPITests.Validators
{
    public class RegisterUserRequestValidatorTests
    {
        private readonly RegisterUserRequestValidator _validator;

        public RegisterUserRequestValidatorTests()
        {
            _validator = new RegisterUserRequestValidator();
        }

        [Fact]
        public void ShouldValidateWithCorrectModel()
        {
            var request = new RegisterUserRequest
            {
                FirstName = "some-name",
                LastName = "some-name",
                Email = "some-email@example.com",
                Password = "some-password"
            };
            var result = _validator.Validate(request);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ShouldNotValidateWhenFirstNameIsMissing()
        {
            var request = new RegisterUserRequest
            {
                LastName = "some-name",
                Email = "some-email@example.com",
                Password = "some-password"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ShouldNotValidateWhenPasswordIsMissing()
        {
            var request = new RegisterUserRequest
            {
                FirstName = "some-name",
                LastName = "some-name",
                Email = "some-email@example.com",
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ShouldNotValidateWhenLastNameIsMissing()
        {
            var request = new RegisterUserRequest
            {
                FirstName = "some-name",
                Email = "some-email@example.com",
                Password = "some-password"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ShouldNotValidateWhenIncorrectEmail()
        {
            var request = new RegisterUserRequest
            {
                FirstName = "some-name",
                LastName = "some-name",
                Email = "some-email-example.com",
                Password = "some-password"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
        }
    }
}