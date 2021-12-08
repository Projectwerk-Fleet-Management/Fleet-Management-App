using System;
using BusinessLayer.Validators;
using BusinessLayer;
using BusinessLayer.Model;
using System.Collections.Generic;
using DAL;

namespace FMA_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Connectionstrings
            //connection string marnick: Data Source=PROTOBEAR\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True
            //connection string Bryan: Data Source=LAPTOP-DGE32LN4\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True
            #endregion
            #region Address testing
            //Address test1 = new(13, "steenbakkerij", "25", "A","Goeferdinge", 9500);
            //Console.WriteLine(testt1.AddressId.ToString() + testt1.Street + testt1.Housenumber + testt1.Addendum + testt1.City + testt1.Postalcode.ToString());

            //Address test2 = new(1, "Steenbakkerij", "21", "a", "Geraardsbergen", 9500);
            //Console.WriteLine(testt.AddressId.ToString() + testt.Street + testt.Housenumber + testt.Addendum + testt.City + testt.Postalcode.ToString());
            #endregion
            #region AddressRepository Testing
            //AddressRepository p = new(@"Data Source=LAPTOP-DGE32LN4\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True");

            #region InsertAddress
            //p.InsertAddress("Steenbakkerij", "21", null, "Geraardsbergen", 9500);
            //p.InsertAddress("Steenbakkerij", "20", null, "Geraardsbergen", 9500);
            //p.InsertAddress("Hogeweg", "15", "A", "Geraardsbergen", 9500);
            #endregion

            #region GetAddress and GetAllAddresses
            /*
            var testAllAddresses = p.GetAllAddresses();

            foreach (var t in testAllAddresses)
            {
                Console.WriteLine($"{t.AddressId} {t.Street} {t.Housenumber} {t.Addendum} {t.City} {t.Postalcode}");
            }
            
            var testtt = p.GetAddress(null, null, null, null, null, 9500);

            foreach (var t in testtt)
            {
                Console.WriteLine($"{t.AddressId} {t.Street} {t.Housenumber} {t.Addendum} {t.City} {t.Postalcode}");
            }

            Console.WriteLine(p.Exists(null, "steenbakkerij", null, null, null, 9500));
            */
            #endregion

            #region UpdateAddress
            //p.UpdateAddress(testAllAddresses[1], test1);
            #endregion

            #region DeleteAddress
            //p.DeleteAddress(testAllAddresses[2]);
            #endregion

            #endregion
            #region FuelcardRepository Testing
            //FuelcardRepository f = new(@"Data Source=LAPTOP-DGE32LN4\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True");

            #region GetAllFuelcards and GetFuelcard Testing
            //var fList = f.GetAllFuelcards();

            //var fList2 = f.GetFuelcard(null, null, "2022-06-25", null, false);
            #endregion

            #region Exists Testing
            //List<Fuel> fTypes1 = new();
            //fTypes1.Add(Fuel.Benzine);
            //List<Fuel> fTypes2 = new();
            //fTypes2.Add(Fuel.Benzine);
            //fTypes2.Add(Fuel.Electrisch);

            //var tst = f.Exists(25, null, null, null, null);
            //Console.WriteLine(f.Exists(8, "189349996715246875", "2022-06-25", fTypes1, false));
            //Console.WriteLine(f.Exists(9, "189349996715246847", "2022-06-25", fTypes2, false));
            #endregion

            #region Insert Testing
            //List<Fuel> fuels = new();
            //fuels.Add(Fuel.Benzine);

            //f.InsertFuelcard("269349996715257874", "2022-06-25", fuels, 1256, true);
            #endregion

            #region Delete Testing
            //List<Fuel> fuels = new();
            //fuels.Add(Fuel.Benzine);
            //Fuelcard fuelcard = new(14, "269349996715246875", new DateTime(2022,06,25), 4865, fuels, true);

            //f.DeleteFuelcard(fuelcard);
            #endregion

            #region Update Testing
            //List<Fuel> oldFuels = new();
            //oldFuels.Add(Fuel.Benzine);

            //List<Fuel> newFuels = new();
            //newFuels.Add(Fuel.Benzine);
            //newFuels.Add(Fuel.Electrisch);

            //Fuelcard oldFuelcard = new(15, "269349996715246874", new DateTime(2022, 06, 30), 1486, newFuels, true);
            //Fuelcard newFuelcard = new(15, "269349996715246874", new DateTime(2022, 06, 30), 1487, newFuels, true);

            //f.UpdateFuelcard(oldFuelcard, newFuelcard);
            #endregion
            #endregion
            #region CarRepository Testing
            //CarRepository c = new(@"Data Source=LAPTOP-DGE32LN4\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True");

            #region GetAllCars and GetCars Testing
            //var allCars = c.GetAllCars();

            //var someCars = c.GetCars(null, null, null, null, "rav 4", null, null, null, null);
            #endregion

            #region Exists Testing
            //var carExists = c.Exists(null, null, null, null, "rav", null, null, null, null);
            #endregion

            #region InsertCar Testing
            //c.InsertCar("1G2ZG57N984137853", "1-xxx-015", "Peugeot", "308", "Family Wagon", new List<Fuel> { Fuel.Benzine}, "4", "Grey");
            #endregion

            #region UpdateCar Testing
            //Car oldCarInfo = new(9, "Peugeot", "308", "1G2ZG57N984137853", "1-xxx-015", "Family Wagon", new List<Fuel> {Fuel.Benzine }, "Grey", "4");
            //Car newCarInfo = new(9, "Peugeot", "308", "1G2ZG57N984137853", "1-xxx-017", "Family Wagon", new List<Fuel> { Fuel.Benzine, Fuel.Electrisch }, "Blue", "4");

            //c.UpdateCar(oldCarInfo, newCarInfo);
            #endregion

            #region DeleteCar Testing
            //Car carToDelete = new(9, "Peugeot", "308", "1G2ZG57N984137853", "1-xxx-017", "Family Wagon", new List<Fuel> { Fuel.Benzine }, "Blue", "4");
            //c.DeleteCar(carToDelete);
            #endregion

            #endregion
            #region DriverRepository testing
            DriverRepository d = new(@"Data Source=.\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True");

            #region GetAllDrivers and GetDrivers Testing
            //var allDrivers = d.GetAllDrivers();

            //foreach (var item in allDrivers)
            //{
            //    Console.WriteLine($"{item.DriverId}, {item.FirstName}, {item.LastName}");
            //}

            //var someDrivers = d.GetDrivers(4, "Bryan", "Batselier", "2002-06-29", "93.11.23-283.87", null);
            #endregion

            #region Exists Testing
            //var existsTrue = d.Exists(4, "Bryan", "Batselier", "2002-06-29", "93.11.23-283.87", null);

            //var existsFalse = d.Exists(5, "Bryan", "Batselier", "2002-06-29", "93.11.23-283.87", null);
            #endregion

            #region Insert Testing
            //d.InsertDriver("Bryan", "Batselier", "2002-06-29", "93.11.23-283.87", null, 1, 1, 1);
            #endregion

            #region Delete Testing
            //Driver driverToDelete = new(4, "Bryan", "Batselier", new DateTime(2002,06,29), "93.11.23-283.87");

            //d.DeleteDriver(driverToDelete);
            #endregion

            #region Update Testing
            //Driver driverToUpdate = new(3, "Marnick", "De Graeve", new DateTime(2002, 06, 29), "00.01.25.567-77", new List<LicenseType> { LicenseType.B});
            //Driver newValues = new(3, "Marnick", "De Graeve", new DateTime(2002, 06, 29), "00.01.25.567-77", new List<LicenseType> { LicenseType.B, LicenseType.A});

            //d.UpdateDriver(driverToUpdate, newValues);
            #endregion

            #endregion
            #region NINValidatior
            //NINValidator x = new NINValidator();
            //bool test = x.isValid("93.11.23-283.87");

            //Driver t = new Driver("-5", "Batselier", "Bryan", new DateTime(1993, 11, 23), "93.11.23-283.85", new List<LicenseType> { LicenseType.A, LicenseType.BE });

            //VINValidator x = new VINValidator();

            //First four give errors back, fifth gives false back and last two give true back
            //Console.WriteLine(x.IsValid(null));
            //Console.WriteLine(x.IsValid("1M8GDM9AKP042788"));
            //Console.WriteLine(x.IsValid("1M8GDM9AQKP042788"));
            //Console.WriteLine(x.IsValid("1M8GDM9A_KP042788"));
            //Console.WriteLine(x.IsValid("1M8GDM9A_KP042798"));
            //Console.WriteLine(x.IsValid("1M8GDM9AXKP042788"));
            //Console.WriteLine(x.IsValid("WAUZZZ8V5KA106598"));
            #endregion
        }
    }
}
