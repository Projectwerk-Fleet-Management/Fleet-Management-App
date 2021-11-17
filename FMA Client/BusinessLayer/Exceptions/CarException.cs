using System;

namespace BusinessLayer.Exceptions
{
    public class CarException : Exception
    {
        public CarException(string message) : base(message)
        {
        }

        public CarException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}