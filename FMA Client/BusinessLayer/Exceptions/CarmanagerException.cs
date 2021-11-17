using System;

namespace BusinessLayer.Exceptions
{
    public class CarmanagerException:Exception
    {
        public CarmanagerException(string message) : base(message)
        {
        }

        public CarmanagerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}