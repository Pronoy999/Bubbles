namespace BubblesAPI.DTOs
{
    public class LoginResponse
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string OrganisationName { get; set; }
        public string Token { get; set; }
    }
}