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
        public int Pincode { private set; get; }
        public List<Fuel> FueltypeList { private set; get; }
        public Driver Driver { private set; get; }
        public bool isActive { private set; get; }

        private FuelcardNumberValidator validator = new FuelcardNumberValidator();

        #region Constructors
        public Fuelcard(int fuelcardId ,string cardnumber, DateTime expiryDate, bool isActive = true)
            : this(fuelcardId, cardnumber, expiryDate, null, null, null, isActive)
        {           
        }

        public Fuelcard(int fuelcardId, string cardnumber, DateTime expiryDate, int? pincode, bool isActive = true)
            : this(fuelcardId, cardnumber, expiryDate, pincode, null, null, isActive)
        {
        }

        public Fuelcard(int fuelcardId, string cardnumber, DateTime expiryDate, List<Fuel> fueltypeList, bool isActive = true)
            : this(fuelcardId, cardnumber, expiryDate, null, fueltypeList, null, isActive)
        {
        }

        public Fuelcard(int fuelcardId, string cardnumber, DateTime expiryDate, Driver driver, bool isActive = true)
            : this(fuelcardId, cardnumber, expiryDate, null, null, driver,isActive)
        {
        }

        public Fuelcard(int fuelcardId, string cardnumber, DateTime expiryDate, int? pincode, List<Fuel> fueltypeList, bool isActive = true)
            : this(fuelcardId, cardnumber, expiryDate, pincode, fueltypeList, null, isActive)
        {
        }

        public Fuelcard(int fuelcardId, string cardnumber, DateTime expiryDate, int? pincode, Driver driver, bool isActive = true)
            : this(fuelcardId, cardnumber, expiryDate, pincode, null, driver, isActive)
        {
        }

        public Fuelcard(int fuelcardId, string cardnumber, DateTime expiryDate, List<Fuel> fueltypeList, Driver driver,bool isActive = true)
            : this(fuelcardId, cardnumber, expiryDate, null, fueltypeList, driver, isActive)
        {
        }

        public Fuelcard(int fuelcardId, string cardnumber, DateTime expiryDate, int? pincode, List<Fuel> fueltypeList, Driver driver, bool isActive = true)
        {
            SetFuelcardId(fuelcardId);
            SetCardnumber(cardnumber);
            SetExpiryDate(expiryDate);
            if (pincode != null) SetPincode(pincode);
            if (fueltypeList != null) SetFueltypeList(fueltypeList);
            if (driver != null) SetDriver(driver);
            SetIsActive(isActive);
        }
        #endregion

        #region Set Methods
        public void SetFuelcardId(int fuelcardId)
        {
            if (fuelcardId <= 0)
            {
                throw new FuelcardException("FuelcardId is not valid");
            }
            this.FuelcardId = fuelcardId;
        }
        public void SetCardnumber(string cardnumber)
        {
            if (cardnumber == null)
            {
                throw new FuelcardException("Cardnumber cannot be null");
            }
            if (!validator.isValid(cardnumber))
            {
                throw new FuelcardException("The entered fuelcardnumber is not valid");
            }
            this.Cardnumber = cardnumber;
        }
        public void SetExpiryDate(DateTime expiryDate)
        {
            
            if (expiryDate < DateTime.Now)
            {
                throw new FuelcardException("This card is already expired");
            }

            this.ExpiryDate = expiryDate;
        }
        public void SetPincode(int? pincode)
        {
            if (pincode < 1000 || pincode > 1000000)
            {
                throw new FuelcardException("Pincode needs to contain at least 4 and can only contain 6 at max");
            }
            this.Pincode = (int)pincode;
        }
        public void SetFueltypeList(List<Fuel> fueltypeList)
        {
            this.FueltypeList = fueltypeList;
        }
        public void SetDriver(Driver driver)
        {
            if (Driver != null) throw new FuelcardException("Driver is already assigned");
            if (driver == null) throw new FuelcardException("Driver cannot be null");
            this.Driver = driver;
            driver.SetFuelcard(this);
        }
        public void SetIsActive(bool isActive)
        {
            this.isActive = isActive;
        }
        #endregion

        #region Add Methods
        public void AddFueltype(Fuel fueltype)
        {
            if (FueltypeList.Contains(fueltype)) throw new FuelcardException("Fuelcard already contains fueltype");
            FueltypeList.Add(fueltype);
        }
        #endregion

        #region Remove Methods
        public void RemoveDriver()
        {
            if (Driver == null) throw new FuelcardException("Fuelcard does not have driver to remove");
            Driver = null;
            Driver.RemoveFuelcard();
        }
        public void RemoveFueltype(Fuel fueltype)
        {
            if (!FueltypeList.Contains(fueltype))
                throw new FuelcardException("Card doesn't contain fueltype to be removed");
            FueltypeList.Remove(fueltype);
        }
        #endregion

        #region Update Methods
        public void UpdatePincode(int pincode)
        {
            if (pincode < 1000 || pincode > 1000000)
            {
                throw new FuelcardException("Pincode needs to contain at least 4 and can only contain 6 at max");
            }
            Pincode = pincode;

        }
        #endregion

        #region Block Methods
        public void BlockFuelcard()
        {
            isActive = false;
        }
        #endregion
    }
}