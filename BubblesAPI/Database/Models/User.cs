using System;

namespace BubblesAPI.Database.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string OrganisationName { get; set; }
        public Status UserStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public Credentials Credentials { get; set; }
    }
}