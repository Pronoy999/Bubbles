using BubblesAPI.DTOs;
using BubblesAPI.Validators;
using Xunit;

namespace BubblesAPITests.Validators
{
    public class LoginRequestValidatorTests
    {
        private readonly LoginRequestValidator _validator;

        public LoginRequestValidatorTests()
        {
            _validator = new LoginRequestValidator();
        }

        [Fact]
        public void ShouldValidateWithCorrectModel()
        {
            var loginRequest = new LoginRequest
            {
                Email = "some-email@example.com",
                Password = "some-password"
            };
            var result = _validator.Validate(loginRequest);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ShouldNotValidateWithIncorrectData()
        {
            var request = new LoginRequest
            {
                Email = "some-email@example.com"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
        }

        [Fact]
        public void ShouldNotValidateWithIncorrectEmail()
        {
            var request = new LoginRequest
            {
                Email = "some-email",
                Password = "some-password"
            };
            var result = _validator.Validate(request);
            Assert.False(result.IsValid);
        }
    }
}