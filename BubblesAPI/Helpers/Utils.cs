using System;

namespace BubblesAPI.Helpers
{
    public class Utils
    {
        public static string GenerateUserId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}