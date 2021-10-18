using System.ComponentModel.DataAnnotations;

namespace BubblesAPI.DTOs
{
    public class RegisterUserRequest
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
        public string Organisation { get; set; }
    }
}