
using System;
using System.Collections.Generic;
using Xunit;
using BusinessLayer;
using BusinessLayer.Model;
using BusinessLayer.Exceptions;

namespace BusinessLayerTests
{
    public class DriverTests
    {
        public List<LicenseType> licenses = new List<LicenseType> {LicenseType.A, LicenseType.BE };
        //public Driver toTest = new("1", "Batselier", "Bryan", new DateTime(1993, 11, 23), "93.11.23-283.87", new List<LicenseType> { LicenseType.A, LicenseType.BE });

        //Tests without car or fuelcard
        [Fact]
        public void Test_Ctor_NoCar_NoFuelcard_Valid()
        {
            Driver toTest = new Driver("1", "Batselier", "Bryan", new DateTime(1993, 11, 23), "93.11.23-283.87", licenses);

            Assert.Equal("1", toTest.Id);
            Assert.Equal("Batselier", toTest.LastName);
            Assert.Equal("Bryan", toTest.FirstName);
            Assert.Equal(new DateTime(1993, 11, 23), toTest.DateOfBirth);
            Assert.Equal("93.11.23-283.87", toTest.NationalIdentificationNumber);
            Assert.Equal(new List<LicenseType> { LicenseType.A, LicenseType.BE }, toTest.Licenses);
        }

        [Theory]
        [InlineData("-10")]
        [InlineData("-0.5")]
        [InlineData("0")]
        public void Test_Ctor_NoCar_NoFuelcard_Invalid(string id)
        {
            Assert.Throws<DriverException>(() => new Driver(id, "Batselier", "Bryan", new DateTime(1993, 11, 23), "93.11.23-283.87", licenses));
        }

        //Tests with car and without fuelcard
        [Fact]
        public void Test_Ctor_Car_NoFuelcard_Valid()
        {
            Driver toTest = new Driver("1", "Batselier", "Bryan", new DateTime(1993, 11, 23), "93.11.23-283.87", licenses, new Car("Toyota", "Yaris", "1HGB41JXMN109186", "1-xxx-000", "Hatchback", Fuel.Benzine));

            Assert.Equal("1", toTest.Id);
            Assert.Equal("Batselier", toTest.LastName);
            Assert.Equal("Bryan", toTest.FirstName);
            Assert.Equal(new DateTime(1993, 11, 23), toTest.DateOfBirth);
            Assert.Equal("93.11.23-283.87", toTest.NationalIdentificationNumber);
            Assert.Equal(new List<LicenseType> { LicenseType.A, LicenseType.BE }, toTest.Licenses);
            //TODO: Check why comparing the entir car object doesn't work
            Assert.Equal("Toyota", toTest.AssignedCar.Make);
        }

        [Theory]
        [InlineData(null)]
        public void Test_Ctor_Car_NoFuelcard_Invalid(Car car)
        {
            Assert.Throws<DriverException>(() => new Driver("1", "Batselier", "Bryan", new DateTime(1993, 11, 23), "93.11.23-283.87", licenses, car));
        }

        //Tests without car and with fuelcard
        [Fact]
         public void Test_Ctor_NoCar_Fuelcard_Valid()
        {
            Driver toTest = new Driver("1", "Batselier", "Bryan", new DateTime(1993, 11, 23), "93.11.23-283.87", licenses, new Fuelcard("012345678901234567",new DateTime(2022,10,13)));

            Assert.Equal("1", toTest.Id);
            Assert.Equal("Batselier", toTest.LastName);
            Assert.Equal("Bryan", toTest.FirstName);
            Assert.Equal(new DateTime(1993, 11, 23), toTest.DateOfBirth);
            Assert.Equal("93.11.23-283.87", toTest.NationalIdentificationNumber);
            Assert.Equal(new List<LicenseType> { LicenseType.A, LicenseType.BE }, toTest.Licenses);
            //TODO: Check why comparing the entire fuelcard object doesn't work
            Assert.Equal("012345678901234567", toTest.AssignedFuelcard.Cardnumber);
        }
        [Theory]
        [InlineData(null)]
        public void Test_Ctor_NoCar_Fuelcard_Invalid(Fuelcard fuelcard)
        {
            Assert.Throws<DriverException>(() => new Driver("1", "Batselier", "Bryan", new DateTime(1993, 11, 23), "93.11.23-283.87", licenses, fuelcard));
        }

        //Tests with car and fuelcard
        [Fact]
        public void Test_Ctor_Car_Fuelcard_Valid()
        {
            Driver toTest = new Driver("1", "Batselier", "Bryan", new DateTime(1993, 11, 23), "93.11.23-283.87", licenses, new Car("Toyota", "Yaris", "1HGB41JXMN109186", "1-xxx-000", "Hatchback", Fuel.Benzine), new Fuelcard("01234567890123456", new DateTime(2022, 10, 13)));

            Assert.Equal("1", toTest.Id);
            Assert.Equal("Batselier", toTest.LastName);
            Assert.Equal("Bryan", toTest.FirstName);
            Assert.Equal(new DateTime(1993, 11, 23), toTest.DateOfBirth);
            Assert.Equal("93.11.23-283.87", toTest.NationalIdentificationNumber);
            Assert.Equal(new List<LicenseType> { LicenseType.A, LicenseType.BE }, toTest.Licenses);
            //TODO: Check why comparing the entire car / fuelcard object doesn't work
            Assert.Equal("Toyota", toTest.AssignedCar.Make);
            Assert.Equal("01234567890123456", toTest.AssignedFuelcard.Cardnumber);
        }
        [Theory]
        [InlineData(null, null)]
        public void Test_Ctor_Car_Fuelcard_Invalid(Car car, Fuelcard fuelcard)
        {
            Assert.Throws<DriverException>(() => new Driver("1", "Batselier", "Bryan", new DateTime(1993, 11, 23), "93.11.23-283.87", licenses, car, fuelcard));
        }
    }
}
