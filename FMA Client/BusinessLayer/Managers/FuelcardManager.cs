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

        public IReadOnlyList<Fuelcard> GetCars()
        {
            try
            {

            }
            catch(Exception exception)
            {
                throw new FuelcardException("", exception);

            }
            return _repo.GetFuelcards();
        }

    }
}