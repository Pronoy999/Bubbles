#nullable enable
using System;

namespace BubblesAPI.Exceptions
{
    [Serializable]
    public class BubblesApiException : Exception
    {
        public BubblesApiException(Exception? innerException) : base(innerException?.Message, innerException)
        {
        }
    }
}