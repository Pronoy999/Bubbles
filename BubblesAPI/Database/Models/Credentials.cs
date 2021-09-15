using System;

namespace BubblesAPI.Database.Models
{
    public class Credentials
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }

        public User User { get; set; }
    }
}