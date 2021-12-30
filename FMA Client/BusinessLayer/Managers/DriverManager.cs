using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Model;

namespace BusinessLayer.Managers
{
    //Ik heb redelijk wat moeten veranderen dat niet echt klopt denk ik
    //Deze manager zal ik wel overpakken
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


        public IReadOnlyList<Driver> GetDrivers(int? driverId, string firstName, string lastName, string dateOfBirth, string nationalIdentificationNumber, List<LicenseType> licenses)
        {
            try
            {
                return _repo.GetDrivers(driverId, firstName, lastName, dateOfBirth, nationalIdentificationNumber, licenses);
            } catch
            {
                throw new DriverManagerException("Failed getting driver list with specific arguments");
            }
        }

        public bool Exists(int? driverId, string firstName, string lastName, string dateOfBirth, string nationalIdentificationNumber, List<LicenseType> licenses)
        {
            try
            {
                return _repo.Exists(driverId, firstName, lastName, dateOfBirth, nationalIdentificationNumber, licenses);
            } catch
            {
                throw new DriverManagerException("Could complete operation 'exists'");
            }
        }

        public void InsertDriver(string firstName, string lastName, string dateOfBirth, string nationalIdentificationNumber, List<LicenseType> licenses
            , int? addressId, int? fuelcardId, int? carId)
        {
            try
            {
                if (!_repo.Exists(null, null, null, null, nationalIdentificationNumber, null))
                {
                    _repo.InsertDriver(firstName, lastName, dateOfBirth, nationalIdentificationNumber, licenses, addressId, fuelcardId, carId);
                }
                else
                {
                    throw new DriverManagerException("Driver already existed");
                }

            }
            catch( Exception e)
            {
                throw new DriverManagerException("Failed to insert driver", e);
            }
        }

        public void DeleteDriver(Driver driver, IAddressRepository iar)
        {
            try
            {
                if (_repo.Exists(driver.DriverId, null, null, null, null, null))
                {
                    AddressManager am = new AddressManager(iar);
                    _repo.DeleteDriver(driver);
                    am.Delete(driver.Address);
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
                if (_repo.Exists(oldDriverInfo.DriverId, null, null, null, null, null))
                {
                    _repo.UpdateDriver(oldDriverInfo, newDriverInfo);
                    if (oldDriverInfo.AssignedFuelcard != newDriverInfo.AssignedFuelcard)
                    {
                        //todo: delete old fuelcard
                    }
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

        public IReadOnlyList<Driver> Search(string x)
        {
            try
            {
                return _repo.Search(x);
            }
            catch
            {
                throw new DriverManagerException("Failed getting driver list with specific arguments");
            }
        }
    }
}