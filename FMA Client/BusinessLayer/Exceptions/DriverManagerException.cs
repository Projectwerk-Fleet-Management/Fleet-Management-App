using System;

namespace BusinessLayer.Exceptions
{
    public class DriverManagerException : Exception
    {
        public DriverManagerException(string message) : base(message)
        {
        }

        public DriverManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}