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
        IReadOnlyList<Driver> GetDrivers(int? driverId, string firstName, string lastName, string dateOfBirth, int? nationalIdentificationNumber, string licenses);
        bool Exists(int? driverId, string firstName, string lastName, string dateOfBirth, int? nationalIdentificationNumber, string licenses);
        void InsertDriver(string firstName, string lastName, string dateOfBirth, int nationalIdentificationNumber, string licenses);
        void DeleteDriver(Driver driver);
        void UpdateDriver(Driver driver);
    }
}
