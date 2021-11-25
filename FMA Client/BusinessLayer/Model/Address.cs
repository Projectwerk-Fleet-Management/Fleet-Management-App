using System.ComponentModel;
using BusinessLayer.Exceptions;

namespace BusinessLayer
{
    public class Address
    {
        public int AddressId { get; private set; }
        public string Street { get; private set; }
        public string Housenumber { get; private set; }
        public string Addendum { get; private set; }
        public string City { get; private set; }
        public int Postalcode { get; private set; }

        //Nullables or this (Constructor chaining)? 
        //Nullables -> string?
        //this (Constructor chaining), prefer this way
        //-> https://codecompiled.com/constructor-chaining-c
        //-> https://stackoverflow.com/questions/10377888/how-can-i-use-multiple-constructors-to-remove-duplicated-code-while-maintaining
        public Address(int addressId, string street, string housenumber, string? addendum, string city, int postalcode)
        {
            SetAddressId(addressId);
            SetStreet(street);
            SetHousenumber(housenumber);
            if (!string.IsNullOrWhiteSpace(addendum)) { SetAddendum(addendum); }; 
            SetCity(city);
            SetPostalcode(postalcode);
        }

        #region Setting of variables
        public void SetAddressId(int addressId)
        {
            if (addressId <= 0) throw new AddressException("AddressId cannot be lower or equel to zero");
            this.AddressId = addressId;
        }
        public void SetStreet(string street)
        {
            if (string.IsNullOrWhiteSpace(street)) throw new AddressException("Street cannot be empty");
            this.Street = street;
        }
        public void SetHousenumber(string housenumber)
        {
            if (string.IsNullOrWhiteSpace(housenumber)) throw new AddressException("Housenumber cannot be empty");
            this.Housenumber = housenumber;
        }
        public void SetAddendum(string addendum)
        {
            if (string.IsNullOrWhiteSpace(addendum)) throw new AddressException("Addendum cannot be empty");
            this.Addendum = addendum;
        }
        public void SetCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city)) throw new AddressException("City cannot be empty");
            this.City = city;
        }
        public void SetPostalcode(int postalcode)
        {
            if (postalcode < 1000 || postalcode > 9992) throw new AddressException("Postalcode is not a valid number");
            this.Postalcode = postalcode;
        }
        #endregion

    }
}