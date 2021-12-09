using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IDriverRepository
    {
        IReadOnlyList<Driver> GetAllDrivers();
        IReadOnlyList<Driver> GetDrivers(int? driverId, string firstName, string lastName, string dateOfBirth, string nationalIdentificationNumber, List<LicenseType> licenses);
        bool Exists(int? driverId, string firstName, string lastName, string dateOfBirth, string nationalIdentificationNumber, List<LicenseType> licenses);
        void InsertDriver(string firstName, string lastName, string dateOfBirth, string nationalIdentificationNumber, List<LicenseType> licenses
            , int? addressId, int? fuelcardId, int? carId);
        void DeleteDriver(Driver driver);
        void UpdateDriver(Driver oldDriverInfo, Driver newDriverInfo);
        IReadOnlyList<Driver> Search(string x);
    }
}
