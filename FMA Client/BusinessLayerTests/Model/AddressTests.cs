using System;
using BusinessLayer;
using BusinessLayer.Exceptions;
using Xunit;
using Xunit.Sdk;

namespace BusinessLayerTests.Model
{
    public class AddressTests
    {
        //(int addressId, string street, string housenumber, string? addendum, string city, int postalcode)


        [Fact]
        public void handleCorrectAddress()
        {

            Address address = new Address(1, "Buizerdlaan", "4", null, "Bornem", 2880, null);
            if (
                address.AddressId == 1 &&
                address.Street == "Buizerdlaan" &&
                address.Housenumber == "4" &&
                address.Addendum == null &&
                address.City == "Bornem" &&
                address.Postalcode == 2880)
            {
                Assert.True(true);
            }
            else
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void handleIncorrectAddress_id()
        {
            Action a = () => new Address(-1, "Buizerdlaan", "4", null, "Bornem", 2880);

            Assert.Throws<AddressException>(a);
        }

        [Fact]
        public void handleIncorrectAddress_street()
        {
            Action a = () => new Address(1, null, "4", null, "Bornem", 2880);

            Assert.Throws<AddressException>(a);
        }

        [Fact]
        public void handleIncorrectAddress_housenumber()
        {
            Action a = () => new Address(1, "Buizerdlaan", null, null, "Bornem", 2880);

            Assert.Throws<AddressException>(a);
        }

        [Fact]
        public void handleIncorrectAddress_city()
        {
            Action a = () => new Address(1, "Buizerdlaan", "4", null, "", 2880);

            Assert.Throws<AddressException>(a);
        }

        [Fact]
        public void handleIncorrectAddress_postcalcode()
        {
            Action a = () => new Address(1, "Buizerdlaan", "4", null, "Bornem", 10);

            Assert.Throws<AddressException>(a);
        }
    }
}
