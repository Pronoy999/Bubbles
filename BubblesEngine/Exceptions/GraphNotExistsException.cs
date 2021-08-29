using System;

namespace BubblesEngine.Exceptions
{
    public class GraphNotExistsException : Exception
    {
        public GraphNotExistsException(string? message) : base(message)
        {
        }
    }
}