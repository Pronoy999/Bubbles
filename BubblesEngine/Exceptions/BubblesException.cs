#nullable enable
using System;

namespace BubblesEngine.Exceptions
{
    [Serializable]
    public class BubblesException : Exception
    {
        public BubblesException(Exception? innerException) : base(innerException?.Message, innerException)
        {
        }
    }
}