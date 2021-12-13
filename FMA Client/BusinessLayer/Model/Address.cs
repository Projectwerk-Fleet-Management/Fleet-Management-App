using System;
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
        public Driver Driver { get; private set; }


        //(Constructor chaining)

        //-> https://codecompiled.com/constructor-chaining-c
        //-> https://stackoverflow.com/questions/10377888/how-can-i-use-multiple-constructors-to-remove-duplicated-code-while-maintaining
        public Address(int addressId, string street, string housenumber, string city, int postalcode)
            : this(addressId, street, housenumber, null, city, postalcode, null)
        {

        }

        public Address(int addressId, string street, string housenumber, string city, int postalcode, Driver driver)
            : this(addressId, street, housenumber, null, city, postalcode, driver)
        {

        }

        public Address(int addressId, string street, string housenumber, string addendum, string city, int postalcode, Driver driver)
        {
            SetAddressId(addressId);
            SetStreet(street);
            SetHousenumber(housenumber);
            if (!string.IsNullOrWhiteSpace(addendum)) { SetAddendum(addendum); }; 
            SetCity(city);
            SetPostalcode(postalcode);
            if (driver != null) SetDriver(driver);
        }

        private void SetDriver(Driver driver)
        {
            if (Driver != null) throw new FuelcardException("Driver is already assigned");
            if (driver == null) throw new FuelcardException("Driver cannot be null");
            this.Driver = driver;
            driver.SetAddress(this);
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