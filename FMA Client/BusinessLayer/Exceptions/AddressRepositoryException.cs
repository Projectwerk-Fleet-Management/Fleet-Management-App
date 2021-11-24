using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Exceptions
{
    public class AddressRepositoryException : Exception
    {
        public AddressRepositoryException(string message) : base(message)
        {
        }

        public AddressRepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
