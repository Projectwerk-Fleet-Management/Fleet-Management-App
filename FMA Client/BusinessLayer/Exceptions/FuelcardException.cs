using System;

namespace BusinessLayer.Exceptions
{
    public class FuelcardException : Exception
    {
        public FuelcardException(string message) : base(message)
        {
        }

        public FuelcardException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}