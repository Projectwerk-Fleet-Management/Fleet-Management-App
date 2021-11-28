using System;
using BusinessLayer;
using BusinessLayer.Exceptions;
using Xunit;

namespace BusinessLayerTests.Model
{
    public class FuelcardTests
    {

        [Fact]
        public void HandleCorrectFuelcard()
        {
            Fuelcard fuelcard = new Fuelcard("000000000000000001", new DateTime(2025, 11, 23));
            if (fuelcard.Cardnumber == "000000000000000001" && fuelcard.ExpiryDate == new DateTime(2025, 11, 23)) ;
        }

        [Fact]
        public void HandleIncorrectFuelcard_cardnumber()
        {
            Action a = () => new Fuelcard("0000000000000000011", new DateTime(2025, 11, 23));
            Assert.Throws<FuelcardException>(a);

        }

        [Fact]
        public void HandleIncorrectFuelcard_expirydateisexpired()
        {
            Action a = () => new Fuelcard("000000000000000001", new DateTime(2000, 11, 23));
            Assert.Throws<FuelcardException>(a);

        }
    }
}