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

        public IReadOnlyList<Fuelcard> GetCards()
        {
            try
            {
                return _repo.GetFuelcards();
            }
            catch(Exception exception)
            {
                throw new FuelcardException("", exception);

            }
        }

    }
}