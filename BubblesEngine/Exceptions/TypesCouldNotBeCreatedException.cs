using System;

namespace BubblesEngine.Exceptions
{
    public class TypesCouldNotBeCreatedException : Exception
    {
        public TypesCouldNotBeCreatedException() : base(ErrorMessages.ErrorMessages.TypeCouldNotBeCreated)
        {
        }
    }
}