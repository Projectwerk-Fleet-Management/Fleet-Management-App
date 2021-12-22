using System;
using System.Collections.Generic;
using System.ComponentModel;
using BusinessLayer.Exceptions;
using BusinessLayer.Validators;

namespace BusinessLayer
{
    public class Car : INotifyPropertyChanged
    {
        public int CarId { private set; get; }
        public string Make { private set; get; }
        public string Model { private set; get; }
        public string Vin { private set; get; }
        public string Licenseplate { private set; get; }
        public string Colour { private set; get; }
        public string Doors { private set; get; }
        public Driver Driver { private set; get; }
        public string Type { private set; get; }
        public List<Fuel> FuelType { private set; get; }

        #region ctors 
        public Car(int carId, string make, string model, string vin, string licenseplate, string type, List<Fuel> fuelType)
            : this(carId, make, model, vin, licenseplate, type, fuelType, null, null, null)
        {

        }
        public Car(int carId, string make, string model, string vin, string licenseplate, string type, List<Fuel> fuelType, Driver driver)
            : this(carId, make, model, vin, licenseplate, type, fuelType, driver, null, null)
        {

        }
        public Car(int carId, string make, string model, string vin, string licenseplate, string type, List<Fuel> fuelType, string colour)
            : this(carId, make, model, vin, licenseplate, type, fuelType, null, colour, null)
        {

        }
        //Signature is different on this one
        public Car(int carId, string make, string model, string vin, string licenseplate, string type, string doors, List<Fuel> fuelType)
            : this(carId, make, model, vin, licenseplate, type, fuelType, null, null, doors)
        {

        }
        public Car(int carId, string make, string model, string vin, string licenseplate, string type, List<Fuel> fuelType, Driver driver, string colour)
            : this(carId, make, model, vin, licenseplate, type, fuelType, driver, colour, null)
        {

        }
        //Signature is different on this one
        public Car(int carId, string make, string model, string vin, string licenseplate, string type, List<Fuel> fuelType, string doors, Driver driver)
            : this(carId, make, model, vin, licenseplate, type, fuelType, driver, null, doors)
        {

        }
        public Car(int carId, string make, string model, string vin, string licenseplate, string type, List<Fuel> fuelType, string colour, string doors)
            : this(carId, make, model, vin, licenseplate, type, fuelType, null, colour, doors)
        {

        }

        //Everything
        public Car(int carId, string make, string model, string vin, string licenseplate, string type, List<Fuel> fuelType,
            Driver driver, string colour, string doors)
        {
            SetCarId(carId);
            SetMake(make);
            SetModel(model);
            SetVIN(vin);
            SetLicenseplate(licenseplate);
            SetType(type);
            SetFueltype(fuelType);
            if (driver != null) SetDriver(driver);
            if (!string.IsNullOrWhiteSpace(colour)) SetColour(colour);
            if (!string.IsNullOrWhiteSpace(doors)) SetDoors(doors);            
        }       
        #endregion

        #region Set methodes
        public void SetCarId(int carId)
        {
            if (carId <= 0) throw new CarException("CarId cannot be lower or equal to zero");
            this.CarId = carId;
            OnPropertyChanged("carId");
        }
        public void SetMake(string make)
        {
            if (string.IsNullOrWhiteSpace(make)) throw new CarException("Make of car is not allowed to be null");
            this.Make = make;
            OnPropertyChanged("Make");
        }
        public void SetModel(string model)
        {
            if (string.IsNullOrWhiteSpace(model)) throw new CarException("Model of a car cannot be null");
            this.Model = model;
            OnPropertyChanged("Model");

        }
        public void SetVIN(string vin)
        {
            VINValidator x = new VINValidator();
            if (!x.IsValid(vin)) throw new CarException("VIN is not valid");
            if (this.Vin != null || this.Vin == "") throw new CarException("You can't change VIN once its been set");
                this.Vin = vin;
            OnPropertyChanged("Vin");

        }
        public void SetLicenseplate(string licenseplate)
        {
            LicenseplateValidator x = new LicenseplateValidator();
            if (string.IsNullOrWhiteSpace(licenseplate)) throw new CarException("Licenseplate cannot be null");
            if (!x.isValid(licenseplate)) throw new CarException("Licenseplate is not valid");
            this.Licenseplate = licenseplate;
            OnPropertyChanged("Licenseplate");
        }
        public void SetDriver(Driver driver)
        {
            if (driver == null) throw new CarException("Driver cannot be null");
            if (Driver != null && Driver != driver) throw new CarException("Driver is already assigned");
            this.Driver = driver;
            if (driver.AssignedCar != this) driver.SetCar(this);
        }
        public void SetColour(string colour)
        {
            if (colour == null) throw new CarException("Colour cannot be null");
            this.Colour = colour;
            OnPropertyChanged("Colour");
        }
        public void SetDoors(string doors)
        {
            if (doors != null && Doors == null)
            {
                Doors = doors;
                OnPropertyChanged("Doors");
            }
            else
            {
                throw new CarException(
                    "Doors could not be set because either Doors was previously set or doors was null");
            }
            
        }
        public void SetType(string type)
        {
            if (string.IsNullOrWhiteSpace(type)) throw new CarException("Type cannot be null");
            this.Type = type;
            OnPropertyChanged("Type");
        }
        public void SetFueltype(List<Fuel> fueltype)
        {
            try
            {
                this.FuelType = fueltype;
                OnPropertyChanged("Fueltype");
            }
            catch (Exception e)
            {
                throw new CarException("Problem occurred setting fueltype", e);
            }
        }
        #endregion

        #region Remove methodes
        public void RemoveDriver()
        {  
            if (Driver.AssignedCar != null)
            {
                Driver.RemoveCar();
            }
            this.Driver = null;
        }
        #endregion

        public override string ToString()
        {
            return $"{Licenseplate}, {Make} {Model}";
        }

        #region INotifypropertychanged members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}