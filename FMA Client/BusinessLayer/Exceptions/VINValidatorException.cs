using System;

namespace BusinessLayer.Exceptions
{
    public class VINValidatorException : Exception
    {
        public VINValidatorException(string message) : base(message)
        {
        }

        public VINValidatorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
