using System;
using System.Collections.Generic;
using System.ComponentModel;
using BusinessLayer.Model;
using BusinessLayer.Exceptions;
using BusinessLayer.Validators;

namespace BusinessLayer
{
    public class Driver
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

        ////Constructor without car, fuelcard, address and licenses
        //public Driver(int driverId, string lastName, string firstName, DateTime dateOfBirth, string nationalIdentificationNumber)
        //{
        //    SetDriverId(driverId);
        //    SetLastName(lastName);
        //    SetFirstName(firstName);
        //    SetDateOfBirth(dateOfBirth);
        //    SetNationalIdentificationNumber(nationalIdentificationNumber);
        //}

        //Constructor without car, fuelcard and address
        public Driver(int driverId, string lastName, string firstName, DateTime dateOfBirth, string nationalIdentificationNumber, List<LicenseType>? licenses, Address? address, Car? assignedCar, Fuelcard? assignedFuelcard)
        {
            SetDriverId(driverId);
            SetLastName(lastName);
            SetFirstName(firstName);
            SetDateOfBirth(dateOfBirth);
            SetNationalIdentificationNumber(nationalIdentificationNumber);
            if (licenses != null) { SetLicenses(licenses); } 
            if (address != null) { SetAddress(address); }
            if (assignedCar != null) { SetCar(assignedCar); }
            if (assignedFuelcard != null) { SetFuelcard(assignedFuelcard); }
        }

        ////Constructor without car and fuelcard
        //public Driver(int driverId, string lastName, string firstName, DateTime dateOfBirth, string nationalIdentificationNumber, List<LicenseType> licenses, Address address)
        //{
        //    SetDriverId(driverId);
        //    SetLastName(lastName);
        //    SetFirstName(firstName);
        //    SetDateOfBirth(dateOfBirth);
        //    SetNationalIdentificationNumber(nationalIdentificationNumber);
        //    SetLicenses(licenses);
        //    SetAddress(address);
        //}

        ////Constructor with car and without fuelcard
        //public Driver(int driverId, string lastName, string firstName, DateTime dateOfBirth, string nationalIdentificationNumber, List<LicenseType> licenses, Car assignedCar, Address address)
        //{
        //    SetDriverId(driverId);
        //    SetLastName(lastName);
        //    SetFirstName(firstName);
        //    SetDateOfBirth(dateOfBirth);
        //    SetNationalIdentificationNumber(nationalIdentificationNumber);
        //    SetLicenses(licenses);
        //    SetCar(assignedCar);
        //    SetAddress(address);
        //}

        ////Constructor without car and with fuelcard
        //public Driver(int driverId, string lastName, string firstName, DateTime dateOfBirth, string nationalIdentificationNumber, List<LicenseType> licenses, Fuelcard assignedFuelcard, Address address)
        //{
        //    SetDriverId(driverId);
        //    SetLastName(lastName);
        //    SetFirstName(firstName);
        //    SetDateOfBirth(dateOfBirth);
        //    SetNationalIdentificationNumber(nationalIdentificationNumber);
        //    SetLicenses(licenses);
        //    SetFuelcard(assignedFuelcard);
        //    SetAddress(address);
        //}

        ////Constructor with car and fuelcard
        //public Driver(int driverId, string lastName, string firstName, DateTime dateOfBirth, string nationalIdentificationNumber, List<LicenseType> licenses, Car assignedCar, Fuelcard assignedFuelcard, Address address)
        //{
        //    SetDriverId(driverId);
        //    SetLastName(lastName);
        //    SetFirstName(firstName);
        //    SetDateOfBirth(dateOfBirth);
        //    SetNationalIdentificationNumber(nationalIdentificationNumber);
        //    SetLicenses(licenses);
        //    SetCar(assignedCar);
        //    SetFuelcard(assignedFuelcard);
        //    SetAddress(address);
        //}

        #region Setting of variables
        public void SetDriverId(int driverId)
        {
            if (driverId <= 0) throw new DriverException("Id is less or equal to zero");
            this.DriverId = driverId;
        }
        public void SetLastName(string lastName)
        {
            if (lastName == null) throw new DriverException("Last name cannot be null");
            this.LastName = lastName;
        }
        public void SetFirstName(string firstName)
        {
            if (firstName == null) throw new DriverException("First name cannot be null");
            this.FirstName = firstName;
        }
        public void SetDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth == null) throw new DriverException("Date of birth cannot be null");
            this.DateOfBirth = dateOfBirth;
        }
        public void SetNationalIdentificationNumber (string nationalIdentificationNumber)
        {
            if (NINValidator.isValid(nationalIdentificationNumber) == false) throw new DriverException("National identification number is not valid");
            this.NationalIdentificationNumber = nationalIdentificationNumber;
        }
        public void SetLicenses (List<LicenseType> licenses)
        {
            if (licenses == null) throw new DriverException("Licenses cannot be null");
            this.Licenses = licenses;
        }
        public void SetCar(Car car)
        {
            if (car == null) throw new DriverException("Car cannot be null");
            AssignedCar = car;
        }
        public void SetFuelcard(Fuelcard fuelcard)
        {
            if (fuelcard == null) throw new DriverException("Fuelcard cannot be null");
            fuelcard.addDriver(this);
            AssignedFuelcard = fuelcard;
        }
        public void SetAddress(Address address)
        {
            if (address == null) throw new DriverException("Address cannot be null");
            Address = address;
        }
        #endregion

        #region Adding of license
        public void AddLicense(LicenseType license)
        {
            if (Licenses.Contains(license)) throw new DriverException("Driver already has this license");
            Licenses.Add(license);
        }
        #endregion

        #region Removing of variables
        public void RemoveFuelcard()
        {
            if (this.AssignedFuelcard == null) throw new DriverException("There is no fuelcard assigned to this driver");
            AssignedFuelcard.removeDriver();
            AssignedFuelcard = null;
        }
        public void RemoveCar()
        {
            if (AssignedCar == null) throw new DriverException("Car is already null");
            AssignedCar.Driver.RemoveCar();
            AssignedCar = null;
        }
        public void RemoveLicense(LicenseType license)
        {
            if (!Licenses.Contains(license)) throw new DriverException("Driver does not have this license");
            Licenses.Remove(license);
        }
        #endregion

    }
}