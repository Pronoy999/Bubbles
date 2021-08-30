using System;

namespace BubblesEngine.Exceptions
{
    public class DatabaseNotFoundException : Exception
    {
        public DatabaseNotFoundException() : base(ErrorMessages.ErrorMessages.DatabaseNotFound)
        {
        }
    }
}