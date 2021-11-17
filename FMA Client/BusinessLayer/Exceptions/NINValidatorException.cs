using System;

namespace BusinessLayer.Exceptions
{
    public class NINValidatorException: Exception
    {
        public NINValidatorException(string message) : base(message)
        {

        }
        public NINValidatorException(string message, Exception innerException) : base(message)
        {

        }
    }
}