using System;

namespace BusinessLayer.Exceptions
{
    public class AddressException: Exception
    {
        public AddressException(string message) : base(message)
        {
        }

        public AddressException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}