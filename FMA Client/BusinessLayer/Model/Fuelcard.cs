﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using BusinessLayer.Exceptions;
using BusinessLayer.Validators;

namespace BusinessLayer
{
    public class Fuelcard : INotifyPropertyChanged
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
            setCardnumber(cardnumber);
            setExpiryDate(expiryDate);
        }

        public Fuelcard(string cardnumber, DateTime expiryDate, string pincode, List<Fuel> fueltypeList,
            Driver driver)
        {
            setCardnumber(cardnumber);
            setExpiryDate(expiryDate);

            Pincode = pincode;
            FueltypeList = fueltypeList;
            Driver = driver;
            isActive = true;
        }

        public Fuelcard(string cardnumber, DateTime expiryDate, string pincode, Fuel fuel,
            Driver driver)
        {
            setCardnumber(cardnumber);
            setExpiryDate(expiryDate);
            FueltypeList.Add(fuel);
            Driver = driver;
            isActive = true;
        }


        public void setExpiryDate(DateTime expiryDate)
        {
            if (expiryDate < DateTime.Now)
            {
                throw new FuelcardException("This card is already expired");
            }
            ExpiryDate = expiryDate;
        }
        public void setCardnumber(string cn)
        {
            if (string.IsNullOrWhiteSpace(cn)) throw new FuelcardException("is empty");
            if (Cardnumber != null) throw new FuelcardException("There already is a cardnumber");
            if (!validator.isValid(cn)) throw new FuelcardException("The entered fuelcardnumber is not valid");
                
            Cardnumber = cn;
        }
        public void addDriver(Driver d)
        {
            if (Driver != null) throw new FuelcardException("Driver is already assigned");
            if (d == null) throw new FuelcardException("Driver cannot be null");
            d.SetFuelcard(this);
            Driver = d;
            OnPropertyChanged("Driver");
        }

        public void removeDriver()
        {
            if (Driver == null) throw new FuelcardException("Fuelcard does not have driver to remove");
            Driver.RemoveFuelcard();
            Driver = null;
            OnPropertyChanged("Driver");
        }

        public void SetPincode(string pincode)
        {
            if (Pincode != null) throw new FuelcardException("There already is a pincode");
            Pincode = pincode;
            OnPropertyChanged("Pincode");
        }

        public void ChangePincode(string pincode)
        {
            if (Pincode == null) throw new FuelcardException("You cannot change an empty pincode");
            Pincode = pincode;
            OnPropertyChanged("Pincode");

        }

        public void AddFueltype(Fuel fueltype)
        {
            if (FueltypeList.Contains(fueltype)) throw new FuelcardException("Fuelcard already contains fueltype"); 
            FueltypeList.Add(fueltype);
            OnPropertyChanged("Fueltypelist");
        }

        public void RetractFueltype(Fuel fueltype)
        {
            if (!FueltypeList.Contains(fueltype))
                throw new FuelcardException("Card doesn't contain fueltype to be removed");
            FueltypeList.Remove(fueltype);
            OnPropertyChanged("Fueltypelist");
        }

        public void BlockFuelcard()
        {
            isActive = false;
            OnPropertyChanged("isActive");
        }

        #region INotifypropertychanged members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}