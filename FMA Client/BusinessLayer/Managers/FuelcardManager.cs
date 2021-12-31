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
            } catch (Exception e)
            {
                throw new FuelcardManagerException("Getting fuelcard list failed" + e.Message);
            }
        }

        public IReadOnlyList<Fuelcard> Filter(string cardnumber, List<Fuel> fueltypes, bool? isActive)
        {
            try
            {
                return _repo.GetFuelcard(null, cardnumber, null, fueltypes, isActive);
            }
            catch
            {
                throw new FuelcardManagerException("Getting fuelcard list failed");
            }
        }

        public IReadOnlyList<Fuelcard> GetFuelcard(string cardnumber)
        {
            try
            {
                return _repo.GetFuelcard(null, cardnumber, null, null, null);
            } catch
            {
                throw new FuelcardManagerException("Getting fuelcard list failed");
            }
        }

        public IReadOnlyList<Fuelcard> Search(string x)
        {
            try
            {
                return _repo.Search(x);
            } catch
            {
                throw new FuelcardManagerException("Getting fuelcard list failed");
            }
        }

        public void Insert(string cardnumber, DateTime expiryDate, List<Fuel> fueltypes,int? pincode,bool isActive)
        {
            try
            {
                _repo.InsertFuelcard(cardnumber, expiryDate, fueltypes, pincode, isActive);
            }
            catch
            {
                throw new FuelcardManagerException("Getting fuelcard list failed");
            }
        }

        public bool Exists(string cardnumber)
        {
            try
            {
                return _repo.Exists(null, cardnumber, null, null, null);
            } catch
            {
                throw new FuelcardManagerException("Getting fuelcard list failed");
            }
        }

        public void UpdateFuelcard(Fuelcard fuelcard, Fuelcard newFuelcard)
        {
            try
            {
                _repo.UpdateFuelcard(fuelcard, newFuelcard);
            } catch
            {
                throw new FuelcardManagerException("Update fuelcard failed");
            }
        }

        //IReadOnlyList<Fuelcard> GetFuelcard(int? fuelcardId, string cardnumber, string expiryDate, List<Fuel> fueltypes, bool? isActive);
        //bool Exists(int? fuelcardId, string cardnumber, string expiryDate, List<Fuel> fueltypes, bool? isActive);
        //void InsertFuelcard(string cardnumber, string expiryDate, List<Fuel> fueltypes, int? pincode, bool? isActive);
        //void DeleteFuelcard(Fuelcard fuelcard);
        //void UpdateFuelcard(Fuelcard oldFuelcard, Fuelcard newFuelcard);

    }
}