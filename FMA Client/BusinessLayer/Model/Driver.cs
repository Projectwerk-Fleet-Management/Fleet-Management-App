using System;
using System.Collections.Generic;
using System.ComponentModel;
using BusinessLayer.Model;
using BusinessLayer.Exceptions;
using BusinessLayer.Validators;

namespace BusinessLayer
{
    public class Driver : INotifyPropertyChanged
    {
        public int DriverId { get; private set; }
        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public Address Address { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string NationalIdentificationNumber { get; private set; }
        public List<LicenseType> Licenses { get; private set; }
        public Car AssignedCar { get; private set; }
        public Fuelcard AssignedFuelcard { get; private set; }
        private static NINValidator NINValidator = new NINValidator();


        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        #region Constructors
        public Driver(int driverId, string firstName, string lastName, DateTime dateOfBirth, string nationalIdentificationNumber)
            :this(driverId, firstName, lastName, null, dateOfBirth, nationalIdentificationNumber, null, null, null)
        {

        }

        public Driver(int driverId, string firstName, string lastName, Address address, DateTime dateOfBirth, string nationalIdentificationNumber)
            : this(driverId, firstName, lastName, address, dateOfBirth, nationalIdentificationNumber, null, null, null)
        {

        }

        public Driver(int driverId, string firstName, string lastName, DateTime dateOfBirth, string nationalIdentificationNumber, List<LicenseType> licenses)
            : this(driverId, firstName, lastName, null, dateOfBirth, nationalIdentificationNumber, licenses, null, null)
        {

        }

        public Driver(int driverId, string firstName, string lastName, DateTime dateOfBirth, string nationalIdentificationNumber, Car car)
            : this(driverId, firstName, lastName, null, dateOfBirth, nationalIdentificationNumber, null, car, null)
        {

        }

        public Driver(int driverId, string firstName, string lastName, DateTime dateOfBirth, string nationalIdentificationNumber, Fuelcard fuelcard)
            : this(driverId, firstName, lastName, null, dateOfBirth, nationalIdentificationNumber, null, null, fuelcard)
        {

        }

        public Driver(int driverId, string firstName, string lastName, Address address, DateTime dateOfBirth, string nationalIdentificationNumber,
            List<LicenseType> licenses)
            : this(driverId, firstName, lastName, address, dateOfBirth, nationalIdentificationNumber, licenses, null, null)
        {

        }

        public Driver(int driverId, string firstName, string lastName, Address address, DateTime dateOfBirth, string nationalIdentificationNumber,
            Car car)
            : this(driverId, firstName, lastName, address, dateOfBirth, nationalIdentificationNumber, null, car, null)
        {

        }

        public Driver(int driverId, string firstName, string lastName, Address address, DateTime dateOfBirth, string nationalIdentificationNumber,
            Fuelcard fuelcard)
            : this(driverId, firstName, lastName, address, dateOfBirth, nationalIdentificationNumber, null, null, fuelcard)
        {

        }

        public Driver(int driverId, string firstName, string lastName, DateTime dateOfBirth, string nationalIdentificationNumber,
            List<LicenseType> licenses, Car car)
            : this(driverId, firstName, lastName, null, dateOfBirth, nationalIdentificationNumber, licenses, car, null)
        {

        }

        public Driver(int driverId, string firstName, string lastName, DateTime dateOfBirth, string nationalIdentificationNumber,
            List<LicenseType> licenses, Fuelcard fuelcard)
            : this(driverId, firstName, lastName, null, dateOfBirth, nationalIdentificationNumber, licenses, null, fuelcard)
        {

        }

        public Driver(int driverId, string firstName, string lastName, Address address, DateTime dateOfBirth, string nationalIdentificationNumber,
            List<LicenseType> licenses, Fuelcard fuelcard)
            : this(driverId, firstName, lastName, address, dateOfBirth, nationalIdentificationNumber, licenses, null, fuelcard)
        {

        }

        public Driver(int driverId, string firstName, string lastName, DateTime dateOfBirth, string nationalIdentificationNumber,
            Car car, Fuelcard fuelcard)
            : this(driverId, firstName, lastName, null, dateOfBirth, nationalIdentificationNumber, null, car, fuelcard)
        {

        }

        public Driver(int driverId, string firstName, string lastName, Address address, DateTime dateOfBirth, string nationalIdentificationNumber,
            List<LicenseType> licenses, Car car, Fuelcard fuelcard)
        {
            SetDriverId(driverId);
            SetFirstName(firstName);
            SetLastName(lastName);
            SetDateOfBirth(dateOfBirth);
            SetNationalIdentificationNumber(nationalIdentificationNumber);
            if (licenses != null) SetLicenses(licenses);
            if (address != null) SetAddress(address);
            if (car != null) SetCar(car);
            if (fuelcard != null) SetFuelcard(fuelcard);
        }
        #endregion

        #region Setting of variables
        public void SetDriverId(int driverId)
        {
            if (driverId <= 0) throw new DriverException("Id is less or equal to zero");
            this.DriverId = driverId;
            OnPropertyChanged("driverID");
        }
        public void SetLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName)) throw new DriverException("Last name cannot be null");
            this.LastName = lastName;
            OnPropertyChanged("lastName");
        }
        public void SetFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new DriverException("First name cannot be null");
            this.FirstName = firstName;
            OnPropertyChanged("firstName");
        }
        public void SetDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth == null) throw new DriverException("Date of birth cannot be null");
            this.DateOfBirth = dateOfBirth;
            OnPropertyChanged("DateOfBirth");
        }
        public void SetNationalIdentificationNumber (string nationalIdentificationNumber)
        {
            if (NINValidator.isValid(nationalIdentificationNumber) == false) throw new DriverException("National identification number is not valid");
            this.NationalIdentificationNumber = nationalIdentificationNumber;
            OnPropertyChanged("NationalIdentificationNumber");
        }
        public void SetLicenses (List<LicenseType> licenses)
        {
            if (licenses == null) throw new DriverException("Licenses cannot be null");
            this.Licenses = licenses;
            OnPropertyChanged("Licenses");
        }
        public void SetCar(Car car)
        {
            if (car == null) throw new DriverException("Car cannot be null");

            if (car.Driver != this) car.SetDriver(this);

            AssignedCar = car;
            OnPropertyChanged("AssignedCar");
        }
        public void SetFuelcard(Fuelcard fuelcard)
        {
            if (fuelcard == null) throw new DriverException("Fuelcard cannot be null");

            if(fuelcard.Driver != this) fuelcard.SetDriver(this);

            AssignedFuelcard = fuelcard;
            OnPropertyChanged("AssignedFuelcard");
        }
        public void SetAddress(Address address)
        {
            if (address == null) throw new DriverException("Address cannot be null");
            Address = address;
            OnPropertyChanged("Address");
        }
        #endregion

        #region Adding of license
        public void AddLicense(LicenseType license)
        {
            if (Licenses.Contains(license)) throw new DriverException("Driver already has this license");
            Licenses.Add(license);
            OnPropertyChanged("Licenses");
        }
        #endregion

        #region Removing of variables
        public void RemoveFuelcard()
        {
            if (this.AssignedFuelcard == null) throw new DriverException("There is no fuelcard assigned to this driver");
            if (AssignedFuelcard.Driver != null) AssignedFuelcard.RemoveDriver();
            AssignedFuelcard = null;
        }
        public void RemoveCar()
        {
            if (AssignedCar == null) throw new DriverException("Car is already null");
            if (AssignedCar.Driver != null) AssignedCar.RemoveDriver();
            AssignedCar = null;
        }
        public void RemoveLicense(LicenseType license)
        {
            if (!Licenses.Contains(license)) throw new DriverException("Driver does not have this license");
            Licenses.Remove(license);
        }
        #endregion

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