using System;

namespace BubblesEngine.Exceptions
{
    public class RelationshipNotFoundException : Exception
    {
        public RelationshipNotFoundException() : base(ErrorMessages.ErrorMessages.RelationshipNotFound)
        {
        }
    }
}