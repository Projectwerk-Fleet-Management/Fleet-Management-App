using System;
using BusinessLayer;
using BusinessLayer.Exceptions;
using Xunit;

namespace BusinessLayerTests.Model
{
    public class CarTests
    {
        private Car car = new Car(1, "Citroen", "Berlingo", "1J4FA24127L200905", "1xxx000", "Bestelwagen", fuelType: Fuel.Diesel);

        [Fact]
        public void HandleCorrectCar()
        {
            if (
                car.carId == 1 &&
                car.Make ==  "Citroen" &&
                car.Model == "Berlingo" &&
                car.Vin == "1J4FA24127L200905" &&
                car.Licenseplate == "1xxx000" &&
                car.Type == "Bestelwagen" &&
                car.FuelType == Fuel.Diesel)
            {
                Assert.True(true);
            }
            else
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void HandleIncorrectCar_id()
        {
            Action a = () => new Car(-1, "Citroen", "Berlingo", "1J4FA24127L200905", "1xxx000", "Bestelwagen", fuelType: Fuel.Diesel);
            Assert.Throws<CarException>(a);
        }

        [Fact]
        public void HandleIncorrectCar_make()
        {
            Action a = () => new Car(1, "", "Berlingo", "1J4FA24127L200905", "1xxx000", "Bestelwagen", fuelType: Fuel.Diesel);
            Assert.Throws<CarException>(a);
        }

        [Fact]
        public void HandleIncorrectCar_model()
        {
            Action a = () => new Car(1, "Citroen", null, "1J4FA24127L200905", "1xxx000", "Bestelwagen", fuelType: Fuel.Diesel);
            Assert.Throws<CarException>(a);
        }

        [Fact]
        public void HandleIncorrectCar_vin()
        {
            Action a = () => new Car(1, "Citroen", "Berlingo", "1G2ZG57@984137853", "1xxx000", "Bestelwagen", fuelType: Fuel.Diesel);
            Assert.Throws<VINValidatorException>(a);
        }

        [Fact]
        public void HandleIncorrectCar_licenseplate()
        {
            Action a = () => new Car(1, "Citroen", "Berlingo", "1J4FA24127L200905", "1xx0x00", "Bestelwagen", fuelType: Fuel.Diesel);
            Assert.Throws<CarException>(a);
        }

        [Fact]
        public void HandleIncorrectCar_type()
        {
            Action a = () => new Car(1, "Citroen", "Berlingo", "1J4FA24127L200905", "1xxx000", "", fuelType: Fuel.Diesel);
            Assert.Throws<CarException>(a);
        }
    }
}