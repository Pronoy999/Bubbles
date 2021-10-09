using System;

namespace BubblesAPI.Exceptions
{
    public class UserNotFoundException:Exception
    {
        public UserNotFoundException():base(ErrorMessages.UserNotFoundException)
        {
        }
    }
}