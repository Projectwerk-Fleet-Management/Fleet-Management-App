using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
using BusinessLayer.Model;

namespace BusinessLayer.Managers
{
    public class DriverManager
    {
        private IDriverRepository _repo;

        public DriverManager(IDriverRepository repo)
        {
            _repo = repo;
        }

        public IReadOnlyList<Driver> DriverList()
        {
            try
            {
                return _repo.GetAllDrivers();
            }
            catch
            {
                throw new DriverManagerException("Getting driver list failed");
            }
        }


        public IReadOnlyList<Driver> DriverList(string? id, string? firstName, string? lastName, DateTime? dateOfBirth, string? nationalIdentificationNumber, string? vin, string? fuelcardNumber, string? license, bool strikt = true)
        {
            try
            {
                return _repo.GetDrivers(id, firstName, lastName, dateOfBirth, nationalIdentificationNumber, vin, fuelcardNumber, license, strikt);
            }
            catch
            {
                throw new DriverManagerException("Failed getting driver list with specific arguments");
            }
        }

        public Driver DriverByID(string id)
        {
            try
            {
                IReadOnlyList<Driver> driverByID = _repo.GetDrivers(id, null, null, null, null, null, null, null, true);
                return driverByID.First();
            }
            catch
            {
                throw new DriverManagerException($"Failed getting driver list with ID {id}");
            }
        }

        public bool Exists(Driver driver)
        {
            try
            {
                return _repo.Exists(driver);
            }
            catch
            {
                throw new DriverManagerException("Could complete operation 'exists'");
            }
        }

        public void InsertDriver(Driver driver)
        {
            try
            {
                _repo.InsertDriver(driver);
            }
            catch
            {
                throw new DriverManagerException("Failed to insert driver");
            }
        }

        public void DeleteDriver(Driver driver)
        {
            try
            {
                _repo.DeleteDriver(driver);
            }
            catch
            {

                throw new DriverManagerException("Could not delete driver");
            }
        }

        public void UpdateDriver(Driver driver)
        {
            try
            {
                _repo.UpdateDriver(driver);
            }
            catch (Exception e)
            {
                throw new DriverManagerException("Could not update driver");
            }
        }
    }
}