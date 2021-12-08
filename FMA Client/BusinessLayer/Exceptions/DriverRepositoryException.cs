using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Exceptions
{
    public class DriverRepositoryException : Exception
    {
        public DriverRepositoryException(string message) : base(message)
        {
        }

        public DriverRepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
