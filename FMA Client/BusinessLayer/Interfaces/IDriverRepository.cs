using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IDriverRepository
    {
        IReadOnlyList<Driver> GetDrivers();
        IReadOnlyList<Driver> GetDrivers(string? id, string? firstName, string? lastName, DateTime? dateOfBirth, string? nationalIdentificationNumber, string? vin, string? fuelcardNumber, string? license, bool strikt = true);
        bool Exists(Driver driver);
        void InsertDriver(Driver driver);
        void DeleteDriver(Driver driver);
        void UpdateDriver(Driver driver);
    }
}
