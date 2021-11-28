using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Exceptions
{
    public class FuelcardRepositoryException : Exception
    {
        public FuelcardRepositoryException(string message) : base(message)
        {
        }

        protected FuelcardRepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
