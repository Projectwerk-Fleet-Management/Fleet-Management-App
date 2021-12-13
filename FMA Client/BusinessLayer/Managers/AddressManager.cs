using System;
using System.Collections.Generic;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;

namespace BusinessLayer.Managers
{
    public class AddressManager
    {

        private IAddressRepository _repo;
        public AddressManager(IAddressRepository repo)
        {
            _repo = repo;
        }

        public IReadOnlyList<Address> GetAllAddresses()
        {
            try
            {
                return _repo.GetAllAddresses();
            }
            catch (Exception e)
            {
                throw new AddressManagerException("failed to get all addresses", e);
            }
        }

        public IReadOnlyList<Address> GetAddress(Address a)
        {
            try
            {
                return _repo.GetAddress(a.AddressId, a.Street, a.Housenumber, a.Addendum, a.City, a.Postalcode);
            }
            catch (Exception e)
            {
                throw new AddressManagerException("cannot get address", e);
            }
        }

        public bool Exists(Address a)
        {
            try
            {
                return _repo.Exists(a.AddressId, a.Street, a.Housenumber, a.Addendum, a.City, a.Postalcode);
            }
            catch (Exception e)
            {

                throw new AddressManagerException("couldn't execute if exists", e);
            }
        }

        public void Insert(Address a)
        {
            try
            {
                if(!Exists(a)) _repo.InsertAddress(a.Street, a.Housenumber, a.Addendum, a.City, a.Postalcode);
            }
            catch (Exception e)
            {
                throw new AddressManagerException("failed to insert", e);
            }
        }

        public void Delete(Address a)
        {
            try
            {
                if(Exists(a)) _repo.DeleteAddress(a);
            }
            catch (Exception e)
            {
                throw new AddressManagerException("could not delete", e);
            }
        }

        public void Update(Address oldAddress, Address newAddress)
        {
            try
            {
                if(Exists(oldAddress)) _repo.UpdateAddress(oldAddress, newAddress);
            }
            catch (Exception e)
            {
                throw new AddressManagerException("failed to update", e);
            }
        }



    }
}