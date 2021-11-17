using System;
using BusinessLayer.Exceptions;
using BusinessLayer.Validators;

namespace BusinessLayer
{
    public class Car
    {
        //test
        public int carId { private set; get; }
        public string Make { private set; get; }
        public string Model { private set; get; }
        public string Vin { private set; get; }
        public string Licenseplate { private set; get; }
        public string Colour { private set; get; }
        public string Doors { private set; get; }
        public Driver Driver { private set; get; }
        public string Type { private set; get; }
        public Fuel FuelType { private set; get; }

        //Everything
        public Car(int carId, string Make, string Model, string Vin, string Licenseplate, string Colour, string Doors,
            Driver Driver, string Type, Fuel FuelType)
        {
            SetCarId(carId);
            SetMake(Make);
            SetModel(Model);
            SetVIN(Vin);
            SetLicenseplate(Licenseplate);
            SetColour(Colour);
            SetDoors(Doors);
            SetDriver(Driver);
            SetType(Type);
            SetFueltype(FuelType);
        }

        //Without colour, doors and driver
        public Car(int carId, string Make, string Model, string Vin, string Licenseplate, string Type, Fuel FuelType)
        {
            SetCarId(carId);
            SetMake(Make);
            SetModel(Model);
            SetVIN(Vin);
            SetLicenseplate(Licenseplate);
            SetType(Type);
            SetFueltype(FuelType);
        }

        //Without doors and driver
        public Car(int carId, string Make, string Model, string Vin, string Licenseplate, string Colour, string Type, Fuel FuelType)
        {
            SetCarId(carId);
            SetMake(Make);
            SetModel(Model);
            SetVIN(Vin);
            SetLicenseplate(Licenseplate);
            SetColour(Colour);
            SetType(Type);
            SetFueltype(FuelType);
        }

        //Without driver
        public Car(int carId, string Make, string Model, string Vin, string Licenseplate, string Colour, string Doors, string Type, Fuel FuelType)
        {
            SetCarId(carId);
            SetMake(Make);
            SetModel(Model);
            SetVIN(Vin);
            SetLicenseplate(Licenseplate);
            SetColour(Colour);
            SetDoors(Doors);
            SetType(Type);
            SetFueltype(FuelType);
        }

        //Without colour and doors
        public Car(int carId, string Make, string Model, string Vin, string Licenseplate, Driver Driver, string Type, Fuel FuelType)
        {
            SetCarId(carId);
            SetMake(Make);
            SetModel(Model);
            SetVIN(Vin);
            SetLicenseplate(Licenseplate);
            SetDriver(Driver);
            SetType(Type);
            SetFueltype(FuelType);
        }

        //Without colour
        public Car(int carId, string Make, string Model, string Vin, string Licenseplate, string Doors, Driver Driver, string Type, Fuel FuelType)
        {
            SetCarId(carId);
            SetMake(Make);
            SetModel(Model);
            SetVIN(Vin);
            SetLicenseplate(Licenseplate);
            SetDoors(Doors);
            SetDriver(Driver);
            SetType(Type);
            SetFueltype(FuelType);
        }

        #region Set methodes
        public void SetCarId(int carId)
        {
            if (carId <= 0) throw new CarException("CarId cannot be lower or equal to zero");
            this.carId = carId;
        }
        public void SetMake(string make)
        {
            if (make == null) throw new CarException("Make of car is not allowed to be null");
            this.Make = make;
        }
        public void SetModel(string model)
        {
            if (model == null) throw new CarException("Model of a car cannot be null");
            this.Model = model;
        }
        public void SetVIN(string vin)
        {
            VINValidator x = new VINValidator();
            if (!x.IsValid(vin)) throw new CarException("VIN is not valid");
            this.Vin = vin;
        }
        public void SetLicenseplate(string licenseplate)
        {
            LicenseplateValidator x = new LicenseplateValidator();
            if (licenseplate == null) throw new CarException("Licenseplate cannot be null");
            if (!x.isValid(licenseplate)) throw new CarException("Licenseplate is not valid");
            this.Licenseplate = licenseplate;
        }
        public void SetDriver(Driver driver)
        {
            if (driver == null) throw new CarException("Driver cannot be null");
            driver.SetCar(this);
            this.Driver = driver;

        }
        public void SetColour(string colour)
        {
            if (colour == null) throw new CarException("Colour cannot be null");
            this.Colour = colour;
        }
        public void SetDoors(string doors)
        {
            if (doors != null && Doors == null)
            {
                Doors = doors;
            }
            else
            {
                throw new CarException(
                    "Doors could not be set because either Doors was previously set or doors was null");
            }
            
        }
        public void SetType(string type)
        {
            if (type == null) throw new CarException("Type cannot be null");
            this.Type = type;
        }
        public void SetFueltype(Fuel fueltype)
        {
            if (fueltype == null) throw new CarException("Fueltype cannot be null");
            this.FuelType = fueltype;
        }
        #endregion

        #region Remove methodes
        public void RemoveDriver()
        {
            Driver.RemoveCar();
            this.Driver = null;
        }
        #endregion
    }
}