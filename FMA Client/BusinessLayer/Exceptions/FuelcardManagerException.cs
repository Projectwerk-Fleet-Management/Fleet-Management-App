using System;

namespace BusinessLayer.Exceptions
{
    public class FuelcardManagerException:Exception 
    {
        public FuelcardManagerException(string message) : base(message)
        {
        }

        public FuelcardManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}