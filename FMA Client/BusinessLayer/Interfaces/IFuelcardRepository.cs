using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IFuelcardRepository
    {
        IReadOnlyList<Fuelcard> GetFuelcards();
        IReadOnlyList<Fuelcard> GetFuelcards(string? cardNumber, DateTime? expiryDate, string? fuelTypes, bool? isActive, bool strikt = true);
        bool Exists();
        void InsertFuelcard();
        void DeleteFuelcard();
        void UpdateFuelcard();
    }
}
