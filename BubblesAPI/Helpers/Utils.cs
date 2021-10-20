using System;
using System.Linq;
using System.Security.Claims;

namespace BubblesAPI.Helpers
{
    public class Utils
    {
        public static string GenerateUserId()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GetUserId(ClaimsPrincipal user)
        {
            return user.Claims.First(c => c.Type == "id").Value;
        }
    }
}