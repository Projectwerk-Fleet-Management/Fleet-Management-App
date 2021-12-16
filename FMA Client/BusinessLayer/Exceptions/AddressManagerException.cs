using System;

namespace BusinessLayer.Exceptions
{
    public class AddressManagerException: Exception
    {
        public AddressManagerException(string message) : base(message)
        {
        }

        public AddressManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}