using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Exceptions
{
    public class CarRepositoryException : Exception
    {
        public CarRepositoryException(string message) : base(message)
        {
        }

        public CarRepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
