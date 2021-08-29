using System;

namespace BubblesEngine.Exceptions
{
    public class TypesCouldNotBeCreatedException : Exception
    {
        public TypesCouldNotBeCreatedException(string? message) : base(message)
        {
        }
    }
}