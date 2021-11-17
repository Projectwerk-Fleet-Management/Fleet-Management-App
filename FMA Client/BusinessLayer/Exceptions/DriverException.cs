using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Exceptions
{
    public class DriverException : Exception
    {
        public DriverException(string message) : base(message)
        {
        }

        public DriverException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
