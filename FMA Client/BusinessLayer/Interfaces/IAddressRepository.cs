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
        IReadOnlyList<Address> GetAddress(int? id, string street, string housenumber, string addendum, string city, int? postalcode);
        bool Exists(int? id, string street, string housenumber, string addendum, string city, int? postalcode);
        void InsertAddress(string street, string housenumber, string addendum, string city, int postalcode);
        void DeleteAddress(Address address);
        void UpdateAddress(Address oldAddressInfo, Address newAddressInfo);
    }
}
