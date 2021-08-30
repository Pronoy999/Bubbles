using System;

namespace BubblesEngine.Exceptions
{
    public class GraphNotFoundException : Exception
    {
        public GraphNotFoundException() : base(ErrorMessages.ErrorMessages.GraphNotFound)
        {
        }
    }
}