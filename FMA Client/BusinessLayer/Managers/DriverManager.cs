using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer.Managers
{

    public class DriverManager
    {
        private IDriverRepository _repo;

        public DriverManager(IDriverRepository repo)
        {
            _repo = repo;
        }

        public IReadOnlyList<Driver> GetAllDrivers()
        {
            try
            {
                return _repo.GetAllDrivers();
            } catch
            {
                throw new DriverManagerException("Getting driver list failed");
            }
        }


        public IReadOnlyList<Driver> GetDrivers(int? driverId, string firstName, string lastName, string dateOfBirth, int? nationalIdentificationNumber, string licenses)
        {
            try
            {
                return _repo.GetDrivers(driverId, firstName, lastName, dateOfBirth, nationalIdentificationNumber, licenses);
            } catch
            {
                throw new DriverManagerException("Failed getting driver list with specific arguments");
            }
        }

        public bool Exists(int? driverId, string firstName, string lastName, string dateOfBirth, int? nationalIdentificationNumber, string licenses)
        {
            try
            {
                return _repo.Exists(driverId, firstName, lastName, dateOfBirth, nationalIdentificationNumber, licenses);
            } catch
            {
                throw new DriverManagerException("Could complete operation 'exists'");
            }
        }

        public void InsertDriver(string firstName, string lastName, string dateOfBirth, int nationalIdentificationNumber, string licenses)
        {
            try
            {
                if (!_repo.Exists(driver))
                {
                    _repo.InsertDriver(driver);
                }
                else
                {
                    throw new DriverManagerException("Driver already existed");
                }

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
                if (_repo.Exists(driver))
                {
                    _repo.DeleteDriver(driver);
                }
                else
                {
                    throw new DriverManagerException("Did not contain driver to be deleted");
                }
                
            }
            catch
            {

                throw new DriverManagerException("Could not delete driver");
            }
        }

        public void UpdateDriver(Driver oldDriverInfo, Driver newDriverInfo)
        {
            try
            {
                if (_repo.Exists(driver))
                {
                    _repo.UpdateDriver(driver);
                }
                else
                {
                    throw new DriverManagerException("Did not contain driver");
                }
                
            }
            catch (Exception e)
            {
                throw new DriverManagerException("Could not update driver", e);
            }
        }
    }
}