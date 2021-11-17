using System;
using System.Collections.Generic;
using BusinessLayer.Exceptions;
using BusinessLayer.Validators;

namespace BusinessLayer
{
    public class Fuelcard
    {
        public int FuelcardId { private set; get; }
        public string Cardnumber { private set; get; }
        public DateTime ExpiryDate { private set; get; }
        public string Pincode { private set; get; }
        public List<Fuel> FueltypeList { private set; get; }
        public Driver Driver { private set; get; }
        public bool isActive { private set; get; }

        private FuelcardNumberValidator validator = new FuelcardNumberValidator();


        //TODO: Add set/remove methodes
        public Fuelcard(string cardnumber, DateTime expiryDate)
        {
            if (cardnumber == null)
            {
                throw new FuelcardException("Cardnumber cannot be null");
            }

            if (!validator.isValid(cardnumber))
            {
                throw new FuelcardException("The entered fuelcardnumber is not valid");
            }
            if (expiryDate < DateTime.Now)
            {
                throw new FuelcardException("This card is already expired");
            }
            Cardnumber = cardnumber;
            ExpiryDate = expiryDate;
        }

        public Fuelcard(string cardnumber, DateTime expiryDate, string pincode, List<Fuel> fueltypeList,
            Driver driver)
        {
            if (cardnumber == null)
            {
                throw new FuelcardException("Cardnumber cannot be null");
            }

            if (!validator.isValid(cardnumber))
            {
                throw new FuelcardException("The entered fuelcardnumber is not valid");
            }
            if (expiryDate < DateTime.Now)
            {
                throw new FuelcardException("This card is already expired");
            }
            Cardnumber = cardnumber;
            ExpiryDate = expiryDate;
            Pincode = pincode;
            FueltypeList = fueltypeList;
            Driver = driver;
            isActive = true;
        }

        public Fuelcard(string cardnumber, DateTime expiryDate, string pincode, Fuel fuel,
            Driver driver)
        {
            if (cardnumber == null)
            {
                throw new FuelcardException("Cardnumber cannot be null");
            }

            if (!validator.isValid(cardnumber))
            {
                throw new FuelcardException("The entered fuelcardnumber is not valid");
            }
            if (expiryDate < DateTime.Now)
            {
                throw new FuelcardException("This card is already expired");
            }
            Cardnumber = cardnumber;
            ExpiryDate = expiryDate;
            Pincode = pincode;
            FueltypeList.Add(fuel);
            Driver = driver;
            isActive = true;
        }

        public void addDriver(Driver d)
        {
            if (Driver != null) throw new FuelcardException("Driver is already assigned");
            if (d == null) throw new FuelcardException("Driver cannot be null");
            d.SetFuelcard(this);
            Driver = d;
        }

        public void removeDriver()
        {
            if (Driver == null) throw new FuelcardException("Fuelcard does not have driver to remove");
            Driver.RemoveFuelcard();
            Driver = null;
        }

        public void SetPincode(string pincode)
        {
            if (Pincode != null) throw new FuelcardException("There already is a pincode");
            Pincode = pincode;
        }

        public void ChangePincode(string pincode)
        {
            if (Pincode == null) throw new FuelcardException("You cannot change an empty pincode");
            Pincode = pincode;

        }

        public void AddFueltype(Fuel fueltype)
        {
            if (FueltypeList.Contains(fueltype)) throw new FuelcardException("Fuelcard already contains fueltype"); 
            FueltypeList.Add(fueltype);
        }

        public void RetractFueltype(Fuel fueltype)
        {
            if (!FueltypeList.Contains(fueltype))
                throw new FuelcardException("Card doesn't contain fueltype to be removed");
            FueltypeList.Remove(fueltype);
        }

        public void BlockFuelcard()
        {
            isActive = false;
        }

    }
}