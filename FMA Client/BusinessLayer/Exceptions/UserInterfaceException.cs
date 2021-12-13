using System;

namespace BusinessLayer.Exceptions
{
    public class UserInterfaceException: Exception
    {
        public UserInterfaceException(string message) : base(message)
        {
        }

        public UserInterfaceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}