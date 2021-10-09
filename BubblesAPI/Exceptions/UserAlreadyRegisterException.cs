using System;

namespace BubblesAPI.Exceptions
{
    public class UserAlreadyRegisterException : Exception
    {
        public UserAlreadyRegisterException() : base(ErrorMessages.UserAlreadyRegisterError)
        {
        }
    }
}