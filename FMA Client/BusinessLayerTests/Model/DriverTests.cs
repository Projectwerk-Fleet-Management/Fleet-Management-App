
using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using Xunit;
using BusinessLayer;
using BusinessLayer.Model;
using BusinessLayer.Exceptions;

namespace BusinessLayerTests.Model
{
    public class DriverTests
    {
        Address address = new Address(1, "Buizerdlaan", "4", null, "Bornem", 2880);
        private List<LicenseType> licenses = new List<LicenseType>
        {
            LicenseType.B,
            LicenseType.A
        };

        private DateTime date = new DateTime(1993, 11, 23);

        //todo: car and assignedfuelcard

        [Fact]
        public void handleCorrectDriver()
        {
            
                Driver driver = new Driver(1, "De Grave", "Marnick", date, "93112328387",
                    licenses,
                    address, null, null);

                if (
                    driver.DriverId == 1 &&
                    driver.LastName == "De Grave" &&
                    driver.FirstName == "Marnick" &&
                    driver.DateOfBirth == date &&
                    driver.Licenses == licenses &&
                    driver.Address == address &&
                    driver.AssignedCar == null &&
                    driver.AssignedFuelcard == null
                )
                {
                    Assert.True(true);
                }
                else
                {
                    Assert.True(false);
                }
        }


        [Fact]
        public void handleinCorrectDriver_id()
        {

           Action a = () => new Driver(-1, "De Grave", "Marnick", new DateTime(1993, 11, 23), "93112328387",
                    licenses,
                    address, null, null);

           Assert.Throws<DriverException>(a);

        }

        [Fact]
        public void handleinCorrectDriver_lastname()
        {

            Action a = () => new Driver(1, "", "Marnick", new DateTime(1993, 11, 23), "93112328387",
                licenses,
                address, null, null);

            Assert.Throws<DriverException>(a);

        }

        [Fact]
        public void handleinCorrectDriver_firstname()
        {

            Action a = () => new Driver(1, "De Grave", null, new DateTime(1993, 11, 23), "93112328387",
                licenses,
                address, null, null);

            Assert.Throws<DriverException>(a);

        }

        [Fact]
        public void handleinCorrectDriver_nin()
        {

            Action a = () => new Driver(1, "De Grave", null, new DateTime(1993, 11, 23), "00012556778",
                licenses,
                address, null, null);

            Assert.Throws<DriverException>(a);

        }

    }
}
