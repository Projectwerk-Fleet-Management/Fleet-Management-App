using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IAddressRepository
    {
        IReadOnlyList<Address> GetAllAddresses();
        IReadOnlyList<Address> GetAddress(int id, bool strikt = true);
        bool Exists(Address address);
        void InsertDriver(Address address);
        void DeleteDriver(Address address);
        void UpdateDriver(Address address);
    }
}
