using System;

namespace BubblesEngine.Exceptions
{
    public class NodeNotFoundException : Exception
    {
        public NodeNotFoundException() : base(ErrorMessages.ErrorMessages.NodeNotFound)
        {
        }
    }
}