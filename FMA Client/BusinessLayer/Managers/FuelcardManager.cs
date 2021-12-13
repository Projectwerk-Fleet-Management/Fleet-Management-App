using System;
using System.Collections.Generic;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;

namespace BusinessLayer.Managers
{
    public class FuelcardManager
    {
        private IFuelcardRepository _repo;

        public FuelcardManager(IFuelcardRepository repo)
        {
            _repo = repo;
        }

        public IReadOnlyList<Fuelcard> GetAllFuelcards()
        {
            try
            {
                return _repo.GetAllFuelcards();
            } catch
            {
                throw new DriverManagerException("Getting fuelcard list failed");
            }
        }


        //IReadOnlyList<Fuelcard> GetFuelcard(int? fuelcardId, string cardnumber, string expiryDate, List<Fuel> fueltypes, bool? isActive);
        //bool Exists(int? fuelcardId, string cardnumber, string expiryDate, List<Fuel> fueltypes, bool? isActive);
        //void InsertFuelcard(string cardnumber, string expiryDate, List<Fuel> fueltypes, int? pincode, bool? isActive);
        //void DeleteFuelcard(Fuelcard fuelcard);
        //void UpdateFuelcard(Fuelcard oldFuelcard, Fuelcard newFuelcard);

    }
}