using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IFuelcardRepository
    {
        IReadOnlyList<Fuelcard> GetAllFuelcards();
        IReadOnlyList<Fuelcard> GetFuelcard(int? fuelcardId, string cardnumber, DateTime? expiryDate, List<Fuel> fueltypes, bool? isActive);
        bool Exists(int? fuelcardId, string cardnumber, DateTime? expiryDate, List<Fuel> fueltypes, bool? isActive);
        void InsertFuelcard(string cardnumber, DateTime expiryDate, List<Fuel> fueltypes, int? pincode, bool? isActive);
        void DeleteFuelcard(Fuelcard fuelcard);
        void UpdateFuelcard(Fuelcard oldFuelcard, Fuelcard newFuelcard);
    }
}
