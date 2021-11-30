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
            #region DriverRepository testing
            //DriverRepository y = new(@"Data Source=LAPTOP-DGE32LN4\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True");

            //y.GetAllDrivers();
            #endregion
            #region FuelcardRepository testing
            FuelcardRepository f = new(@"Data Source=LAPTOP-DGE32LN4\SQLEXPRESS;Initial Catalog=fmaDatabase;Integrated Security=True");

            #region GetAllFuelcards and GetFuelcard Checking
            var fList = f.GetAllFuelcards();

            var fList2 = f.GetFuelcard(null, null, "2022-06-25", null, false);
            #endregion

            #region Exists Testing
            //List<Fuel> fTypes1 = new();
            //fTypes1.Add(Fuel.Benzine);
            //List<Fuel> fTypes2 = new();
            //fTypes2.Add(Fuel.Benzine);
            //fTypes2.Add(Fuel.Electrisch);

            //Console.WriteLine(f.Exists(8, "189349996715246875", "2022-06-25", fTypes1, false));
            //Console.WriteLine(f.Exists(9, "189349996715246847", "2022-06-25", fTypes2, false));
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
