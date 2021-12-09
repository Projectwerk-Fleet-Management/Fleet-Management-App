using BusinessLayer;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
using BusinessLayer.Model;
using BusinessLayer.Validators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class DriverRepository : IDriverRepository
    {
        private string _connectionString;
        private NINValidator ninValidator = new();

        public DriverRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            return connection;
        }

        //Done
        public IReadOnlyList<Driver> GetAllDrivers()
        {
            List<Driver> drivers = new();
            SqlConnection connection = getConnection();
            string query = "SELECT *, C.Fueltypes AS carFueltypes, F.Fueltypes AS fuelcardFueltypes FROM dbo.Driver AS D" +
                " LEFT JOIN dbo.Address AS A ON D.AddressId = A.AddressId" +
                " LEFT JOIN dbo.Car AS C ON D.CarId = C.CarId" +
                " LEFT JOIN dbo.Fuelcard AS F ON D.FuelcardId = F.FuelcardId;";

            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    SqlDataReader datareader = command.ExecuteReader();
                    while (datareader.Read())
                    {
                        int driverId = (int)datareader["DriverId"];
                        string firstName = (string)datareader["FirstName"];
                        string lastName = (string)datareader["LastName"];
                        DateTime dateOfBirth = (DateTime)datareader["DateOfBirth"];
                        string nationalIdentificationNumber = (string)datareader["NationalIdentificationNumber"];
                        List<LicenseType> licensesList = new();
                        Address address = null;
                        Car car = null;
                        Fuelcard fuelcard = null;
                        #region Null checking
                        if (DBNull.Value != datareader["Licenses"])
                        {
                            #region Db licenses to program
                            //In the database the licenses are put in as a string with a "," seperating each type.
                            //This basically turns it into a list for us to use
                            //We're also making the licenses all upper case in the array to eliminate the case sensitivity
                            string licensesString = (string)datareader["Licenses"];
                            licensesString = licensesString.Replace(" ", "");
                            string[] licensesArray = licensesString.ToUpper().Split(",");

                            foreach (var l in licensesArray)
                            {
                                if (string.IsNullOrWhiteSpace(l)) { continue; }
                                if (l == LicenseType.A.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.A);
                                }
                                if (l == LicenseType.A1.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.A1);
                                }
                                if (l == LicenseType.A2.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.A2);
                                }
                                if (l == LicenseType.AM.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.AM);
                                }
                                if (l == LicenseType.B.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.B);
                                }
                                if (l == LicenseType.BE.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.BE);
                                }
                                if (l == LicenseType.C.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.C);
                                }
                                if (l == LicenseType.CE.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.CE);
                                }
                                if (l == LicenseType.C1.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.C1);
                                }
                                if (l == LicenseType.C1E.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.C1E);
                                }
                                if (l == LicenseType.D.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.D);
                                }
                                if (l == LicenseType.DE.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.DE);
                                }
                                if (l == LicenseType.D1.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.D1);
                                }
                                if (l == LicenseType.D1E.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.D1E);
                                }
                                if (l == LicenseType.G.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.G);
                                }
                            }
                            #endregion
                        }
                        if (DBNull.Value != datareader["AddressId"])
                        {
                            int addressId = (int)datareader["AddressId"];
                            string street = (string)datareader["Street"];
                            string housenumber = (string)datareader["Housenumber"];
                            string addendum = null;
                            if (DBNull.Value != datareader["Addendum"])
                            {
                                addendum = (string)datareader["Addendum"];
                            }
                            int postalcode = (int)datareader["Postalcode"];
                            string city = (string)datareader["City"];
                            address = new(addressId, street, housenumber, addendum, city, postalcode, null);
                        }
                        if (DBNull.Value != datareader["CarId"])
                        {
                            int carId = (int)datareader["CarId"];
                            string make = (string)datareader["Make"];
                            string model = (string)datareader["Model"];
                            string vin = (string)datareader["Vin"];
                            string licenseplate = (string)datareader["Licenseplate"];
                            string vehicleType = (string)datareader["VehicleType"];
                            string fueltypesString = (string)datareader["carFueltypes"];
                            string doors = "";
                            string colour = "";
                            if (DBNull.Value != datareader["Doors"])
                            {
                                doors = (string)datareader["Doors"];
                            }
                            if (DBNull.Value != datareader["Colour"])
                            {
                                colour = (string)datareader["Colour"];
                            }

                            #region Fueltypes from string (db) to a list
                            List<Fuel> fueltypesList = new();
                            fueltypesString = fueltypesString.Replace(" ", "");
                            string[] fueltypesArray = fueltypesString.ToUpper().Split(",");

                            foreach (var f in fueltypesArray)
                            {
                                if (string.IsNullOrWhiteSpace(f)) { continue; }
                                if (f == Fuel.Benzine.ToString().ToUpper())
                                {
                                    fueltypesList.Add(Fuel.Benzine);
                                }
                                if (f == Fuel.Diesel.ToString().ToUpper())
                                {
                                    fueltypesList.Add(Fuel.Diesel);
                                }
                                if (f == Fuel.Electrisch.ToString().ToUpper())
                                {
                                    fueltypesList.Add(Fuel.Electrisch);
                                }
                            }
                            #endregion

                            car = new(carId, make, model, vin, licenseplate, vehicleType, fueltypesList, colour, doors);
                        }
                        if (DBNull.Value != datareader["FuelcardId"])
                        {

                            int fuelcardId = (int)datareader["FuelcardId"];
                            string cardnumber = (string)datareader["Cardnumber"];
                            DateTime expiryDate = (DateTime)datareader["ExpiryDate"];
                            bool isActive = (bool)datareader["IsActive"];
                            List<Fuel> fueltypesList = new();

                            if (DBNull.Value != datareader["fuelcardFueltypes"])
                            {
                                #region Db fueltype to program
                                //In the database the fueltypes are put in as a string with a "," seperating each type.
                                //This basically turns it into a list for us to use
                                //We're also making the fueltypes all upper case in the array to eliminate the case sensitivity
                                string fueltypesString = (string)datareader["fuelcardFueltypes"];
                                fueltypesString = fueltypesString.Replace(" ", "");
                                string[] fueltypesArray = fueltypesString.ToUpper().Split(",");

                                foreach (var f in fueltypesArray)
                                {
                                    if (string.IsNullOrWhiteSpace(f)) { continue; }
                                    if (f == Fuel.Benzine.ToString().ToUpper())
                                    {
                                        fueltypesList.Add(Fuel.Benzine);
                                    }
                                    if (f == Fuel.Diesel.ToString().ToUpper())
                                    {
                                        fueltypesList.Add(Fuel.Diesel);
                                    }
                                    if (f == Fuel.Electrisch.ToString().ToUpper())
                                    {
                                        fueltypesList.Add(Fuel.Electrisch);
                                    }
                                }
                                #endregion
                            }
                            /* Pincode is not enabled to be retrieved from the database because of course why would it be? 
                            if (DBNull.Value != datareader["Pincode"])
                            {
                                int pincode = (int)datareader["Pincode"];
                            }
                            */

                            fuelcard = new(fuelcardId, cardnumber, expiryDate, /*pincode,*/ fueltypesList, isActive);
                        }
                        #endregion

                        Driver driver = new(driverId, firstName, lastName, address, dateOfBirth, nationalIdentificationNumber, licensesList, car, fuelcard);

                        drivers.Add(driver);
                    }
                    datareader.Close();

                } catch (Exception ex)
                {
                    throw new DriverRepositoryException("GetAllDrivers failed" + ex.Message);
                } finally
                {
                    connection.Close();
                }

                return drivers;
            }
        }

        //Done
        public IReadOnlyList<Driver> GetDrivers(int? driverId, string firstName, string lastName, string dateOfBirth, string nationalIdentificationNumber
            , List<LicenseType> licenses)
        {
            bool AND = false;
            List<Driver> drivers = new();
            SqlConnection connection = getConnection();
            string query = "SELECT *, C.Fueltypes AS carFueltypes, F.Fueltypes AS fuelcardFueltypes FROM dbo.Driver AS D " +
                            " LEFT JOIN dbo.Address AS A ON D.AddressId = A.AddressId" +
                            " LEFT JOIN dbo.Car AS C ON D.CarId = C.CarId" +
                            " LEFT JOIN dbo.Fuelcard AS F ON D.FuelcardId = F.FuelcardId ";

            #region Add to the query for search function
            if (driverId != null)
            {
                query += "WHERE D.DriverId = @Id ";
                AND = true;
            }
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                if (AND == true)
                {
                    query += "AND D.FirstName = @FirstName ";
                } else
                {
                    query += "WHERE D.FirstName = @FirstName ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                if (AND == true)
                {
                    query += "AND D.LastName = @LastName ";
                } else
                {
                    query += "WHERE D.LastName = @LastName ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(dateOfBirth))
            {
                if (AND == true)
                {
                    query += "AND D.DateOfBirth = @DateOfBirth ";
                } else
                {
                    query += "WHERE D.DateOfBirth = @DateOfBirth ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(nationalIdentificationNumber))
            {
                if (AND == true)
                {
                    query += "AND D.NationalIdentificationNumber = @NationalIdentificationNumber ";
                } else
                {
                    query += "WHERE D.NationalIdentificationNumber = @NationalIdentificationNumber ";
                    AND = true;
                }
            }
            if (licenses != null)
            {
                if (AND == true)
                {
                    query += "AND D.Licenses = @Licenses ";
                } else
                {
                    query += "WHERE D.Licenses = @Licenses ";
                    AND = true;
                }
            }
            #endregion


            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    #region Add values to search terms
                    if (driverId != null) command.Parameters.AddWithValue("@Id", driverId);
                    if (!string.IsNullOrWhiteSpace(firstName)) command.Parameters.AddWithValue("@FirstName", firstName);
                    if (!string.IsNullOrWhiteSpace(lastName)) command.Parameters.AddWithValue("@LastName", lastName);
                    if (!string.IsNullOrWhiteSpace(dateOfBirth)) command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    if (!string.IsNullOrWhiteSpace(nationalIdentificationNumber)) command.Parameters.AddWithValue("@NationalIdentificationNumber", nationalIdentificationNumber);
                    string licensesSearch = "";
                    bool first = true;
                    if (licenses != null)
                    {
                        foreach (var l in licenses)
                        {
                            if (l == LicenseType.A)
                            {
                                if (first) { licensesSearch += "A"; first = false; } else { licensesSearch += ", A"; }
                            }
                            if (l == LicenseType.A1)
                            {
                                if (first) { licensesSearch += "A1"; first = false; } else { licensesSearch += ", A1"; }
                            }
                            if (l == LicenseType.A2)
                            {
                                if (first) { licensesSearch += "A2"; first = false; } else { licensesSearch += ", A2"; }
                            }
                            if (l == LicenseType.AM)
                            {
                                if (first) { licensesSearch += "AM"; first = false; } else { licensesSearch += ", AM"; }
                            }
                            if (l == LicenseType.B)
                            {
                                if (first) { licensesSearch += "B"; first = false; } else { licensesSearch += ", B"; }
                            }
                            if (l == LicenseType.BE)
                            {
                                if (first) { licensesSearch += "BE"; first = false; } else { licensesSearch += ", BE"; }
                            }
                            if (l == LicenseType.C)
                            {
                                if (first) { licensesSearch += "C"; first = false; } else { licensesSearch += ", C"; }
                            }
                            if (l == LicenseType.CE)
                            {
                                if (first) { licensesSearch += "CE"; first = false; } else { licensesSearch += ", CE"; }
                            }
                            if (l == LicenseType.C1)
                            {
                                if (first) { licensesSearch += "C1"; first = false; } else { licensesSearch += ", C1"; }
                            }
                            if (l == LicenseType.C1E)
                            {
                                if (first) { licensesSearch += "C1E"; first = false; } else { licensesSearch += ", C1E"; }
                            }
                            if (l == LicenseType.D)
                            {
                                if (first) { licensesSearch += "D"; first = false; } else { licensesSearch += ", D"; }
                            }
                            if (l == LicenseType.DE)
                            {
                                if (first) { licensesSearch += "DE"; first = false; } else { licensesSearch += ", DE"; }
                            }
                            if (l == LicenseType.D1)
                            {
                                if (first) { licensesSearch += "D1"; first = false; } else { licensesSearch += ", D1"; }
                            }
                            if (l == LicenseType.D1E)
                            {
                                if (first) { licensesSearch += "D1E"; first = false; } else { licensesSearch += ", D1E"; }
                            }
                            if (l == LicenseType.G)
                            {
                                if (first) { licensesSearch += "G"; first = false; } else { licensesSearch += ", G"; }
                            }
                        }

                        command.Parameters.AddWithValue("@Licenses", licensesSearch);
                    }
                    #endregion
                    command.CommandText = query;
                    SqlDataReader datareader = command.ExecuteReader();
                    while (datareader.Read())
                    {
                        int driverIdDb = (int)datareader["DriverId"];
                        string firstNameDb = (string)datareader["FirstName"];
                        string lastNameDb = (string)datareader["LastName"];
                        DateTime dateOfBirthDb = (DateTime)datareader["DateOfBirth"];
                        string nationalIdentificationNumberDb = (string)datareader["NationalIdentificationNumber"];
                        List<LicenseType> licensesList = new();
                        Address address = null;
                        Car car = null;
                        Fuelcard fuelcard = null;
                        #region Null checking
                        if (DBNull.Value != datareader["Licenses"])
                        {
                            #region Db licenses to program
                            //In the database the licenses are put in as a string with a "," seperating each type.
                            //This basically turns it into a list for us to use
                            //We're also making the licenses all upper case in the array to eliminate the case sensitivity
                            string licensesString = (string)datareader["Licenses"];
                            licensesString = licensesString.Replace(" ", "");
                            string[] licensesArray = licensesString.ToUpper().Split(",");

                            foreach (var l in licensesArray)
                            {
                                if (string.IsNullOrWhiteSpace(l)) { continue; }
                                if (l == LicenseType.A.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.A);
                                }
                                if (l == LicenseType.A1.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.A1);
                                }
                                if (l == LicenseType.A2.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.A2);
                                }
                                if (l == LicenseType.AM.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.AM);
                                }
                                if (l == LicenseType.B.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.B);
                                }
                                if (l == LicenseType.BE.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.BE);
                                }
                                if (l == LicenseType.C.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.C);
                                }
                                if (l == LicenseType.CE.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.CE);
                                }
                                if (l == LicenseType.C1.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.C1);
                                }
                                if (l == LicenseType.C1E.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.C1E);
                                }
                                if (l == LicenseType.D.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.D);
                                }
                                if (l == LicenseType.DE.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.DE);
                                }
                                if (l == LicenseType.D1.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.D1);
                                }
                                if (l == LicenseType.D1E.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.D1E);
                                }
                                if (l == LicenseType.G.ToString().ToUpper())
                                {
                                    licensesList.Add(LicenseType.G);
                                }
                            }
                            #endregion
                        }
                        if (DBNull.Value != datareader["AddressId"])
                        {
                            int addressId = (int)datareader["AddressId"];
                            string street = (string)datareader["Street"];
                            string housenumber = (string)datareader["Housenumber"];
                            string addendum = null;
                            if (DBNull.Value != datareader["Addendum"])
                            {
                                addendum = (string)datareader["Addendum"];
                            }
                            int postalcode = (int)datareader["Postalcode"];
                            string city = (string)datareader["City"];
                            address = new(addressId, street, housenumber, addendum, city, postalcode, null);
                        }
                        if (DBNull.Value != datareader["CarId"])
                        {
                            int carId = (int)datareader["CarId"];
                            string make = (string)datareader["Make"];
                            string model = (string)datareader["Model"];
                            string vin = (string)datareader["Vin"];
                            string licenseplate = (string)datareader["Licenseplate"];
                            string vehicleType = (string)datareader["VehicleType"];
                            string fueltypesString = (string)datareader["carFueltypes"];
                            string doors = "";
                            string colour = "";
                            if (DBNull.Value != datareader["Doors"])
                            {
                                doors = (string)datareader["Doors"];
                            }
                            if (DBNull.Value != datareader["Colour"])
                            {
                                colour = (string)datareader["Colour"];
                            }

                            #region Fueltypes from string (db) to a list
                            List<Fuel> fueltypesList = new();
                            fueltypesString = fueltypesString.Replace(" ", "");
                            string[] fueltypesArray = fueltypesString.ToUpper().Split(",");

                            foreach (var f in fueltypesArray)
                            {
                                if (string.IsNullOrWhiteSpace(f)) { continue; }
                                if (f == Fuel.Benzine.ToString().ToUpper())
                                {
                                    fueltypesList.Add(Fuel.Benzine);
                                }
                                if (f == Fuel.Diesel.ToString().ToUpper())
                                {
                                    fueltypesList.Add(Fuel.Diesel);
                                }
                                if (f == Fuel.Electrisch.ToString().ToUpper())
                                {
                                    fueltypesList.Add(Fuel.Electrisch);
                                }
                            }
                            #endregion

                            car = new(carId, make, model, vin, licenseplate, vehicleType, fueltypesList, colour, doors);
                        }
                        if (DBNull.Value != datareader["FuelcardId"])
                        {

                            int fuelcardId = (int)datareader["FuelcardId"];
                            string cardnumber = (string)datareader["Cardnumber"];
                            DateTime expiryDate = (DateTime)datareader["ExpiryDate"];
                            bool isActive = (bool)datareader["IsActive"];
                            List<Fuel> fueltypesList = new();

                            if (DBNull.Value != datareader["fuelcardFueltypes"])
                            {
                                #region Db fueltype to program
                                //In the database the fueltypes are put in as a string with a "," seperating each type.
                                //This basically turns it into a list for us to use
                                //We're also making the fueltypes all upper case in the array to eliminate the case sensitivity
                                string fueltypesString = (string)datareader["fuelcardFueltypes"];
                                fueltypesString = fueltypesString.Replace(" ", "");
                                string[] fueltypesArray = fueltypesString.ToUpper().Split(",");

                                foreach (var f in fueltypesArray)
                                {
                                    if (string.IsNullOrWhiteSpace(f)) { continue; }
                                    if (f == Fuel.Benzine.ToString().ToUpper())
                                    {
                                        fueltypesList.Add(Fuel.Benzine);
                                    }
                                    if (f == Fuel.Diesel.ToString().ToUpper())
                                    {
                                        fueltypesList.Add(Fuel.Diesel);
                                    }
                                    if (f == Fuel.Electrisch.ToString().ToUpper())
                                    {
                                        fueltypesList.Add(Fuel.Electrisch);
                                    }
                                }
                                #endregion
                            }
                            /* Pincode is not enabled to be retrieved from the database because of course why would it be? 
                            if (DBNull.Value != datareader["Pincode"])
                            {
                                int pincode = (int)datareader["Pincode"];
                            }
                            */

                            fuelcard = new(fuelcardId, cardnumber, expiryDate, /*pincode,*/ fueltypesList, isActive);
                        }
                        #endregion

                        Driver driver = new(driverIdDb, firstNameDb, lastNameDb, address, dateOfBirthDb, nationalIdentificationNumberDb, licensesList, car, fuelcard);

                        drivers.Add(driver);
                    }
                    datareader.Close();

                } catch (Exception ex)
                {
                    throw new DriverRepositoryException("GetDrivers failed" + ex.Message);
                } finally
                {
                    connection.Close();
                }

                return drivers;
            }
        }

        //Done
        public bool Exists(int? driverId, string firstName, string lastName, string dateOfBirth, string nationalIdentificationNumber, List<LicenseType> licenses)
        {
            bool AND = false;
            List<Driver> drivers = new();
            SqlConnection connection = getConnection();
            string query = "SELECT Count(*) FROM dbo.Driver ";

            #region Add to the query for search function
            if (driverId != null)
            {
                query += "WHERE DriverId = @Id ";
                AND = true;
            }
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                if (AND == true)
                {
                    query += "AND FirstName = @FirstName ";
                } else
                {
                    query += "WHERE FirstName = @FirstName ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                if (AND == true)
                {
                    query += "AND LastName = @LastName ";
                } else
                {
                    query += "WHERE LastName = @LastName ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(dateOfBirth))
            {
                if (AND == true)
                {
                    query += "AND DateOfBirth = @DateOfBirth ";
                } else
                {
                    query += "WHERE DateOfBirth = @DateOfBirth ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(nationalIdentificationNumber))
            {
                if (AND == true)
                {
                    query += "AND NationalIdentificationNumber = @NationalIdentificationNumber ";
                } else
                {
                    query += "WHERE NationalIdentificationNumber = @NationalIdentificationNumber ";
                    AND = true;
                }
            }
            if (licenses != null)
            {
                if (AND == true)
                {
                    query += "AND Licenses = @Licenses ";
                } else
                {
                    query += "WHERE Licenses = @Licenses ";
                    AND = true;
                }
            }
            #endregion


            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    #region Add values to search terms
                    if (driverId != null) command.Parameters.AddWithValue("@Id", driverId);
                    if (!string.IsNullOrWhiteSpace(firstName)) command.Parameters.AddWithValue("@FirstName", firstName);
                    if (!string.IsNullOrWhiteSpace(lastName)) command.Parameters.AddWithValue("@LastName", lastName);
                    if (!string.IsNullOrWhiteSpace(dateOfBirth)) command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    if (!string.IsNullOrWhiteSpace(nationalIdentificationNumber)) command.Parameters.AddWithValue("@NationalIdentificationNumber", nationalIdentificationNumber);
                    string licensesSearch = "";
                    bool first = true;
                    if (licenses != null)
                    {
                        foreach (var l in licenses)
                        {
                            if (l == LicenseType.A)
                            {
                                if (first) { licensesSearch += "A"; first = false; } else { licensesSearch += ", A"; }
                            }
                            if (l == LicenseType.A1)
                            {
                                if (first) { licensesSearch += "A1"; first = false; } else { licensesSearch += ", A1"; }
                            }
                            if (l == LicenseType.A2)
                            {
                                if (first) { licensesSearch += "A2"; first = false; } else { licensesSearch += ", A2"; }
                            }
                            if (l == LicenseType.AM)
                            {
                                if (first) { licensesSearch += "AM"; first = false; } else { licensesSearch += ", AM"; }
                            }
                            if (l == LicenseType.B)
                            {
                                if (first) { licensesSearch += "B"; first = false; } else { licensesSearch += ", B"; }
                            }
                            if (l == LicenseType.BE)
                            {
                                if (first) { licensesSearch += "BE"; first = false; } else { licensesSearch += ", BE"; }
                            }
                            if (l == LicenseType.C)
                            {
                                if (first) { licensesSearch += "C"; first = false; } else { licensesSearch += ", C"; }
                            }
                            if (l == LicenseType.CE)
                            {
                                if (first) { licensesSearch += "CE"; first = false; } else { licensesSearch += ", CE"; }
                            }
                            if (l == LicenseType.C1)
                            {
                                if (first) { licensesSearch += "C1"; first = false; } else { licensesSearch += ", C1"; }
                            }
                            if (l == LicenseType.C1E)
                            {
                                if (first) { licensesSearch += "C1E"; first = false; } else { licensesSearch += ", C1E"; }
                            }
                            if (l == LicenseType.D)
                            {
                                if (first) { licensesSearch += "D"; first = false; } else { licensesSearch += ", D"; }
                            }
                            if (l == LicenseType.DE)
                            {
                                if (first) { licensesSearch += "DE"; first = false; } else { licensesSearch += ", DE"; }
                            }
                            if (l == LicenseType.D1)
                            {
                                if (first) { licensesSearch += "D1"; first = false; } else { licensesSearch += ", D1"; }
                            }
                            if (l == LicenseType.D1E)
                            {
                                if (first) { licensesSearch += "D1E"; first = false; } else { licensesSearch += ", D1E"; }
                            }
                            if (l == LicenseType.G)
                            {
                                if (first) { licensesSearch += "G"; first = false; } else { licensesSearch += ", G"; }
                            }
                        }

                        command.Parameters.AddWithValue("@Licenses", licensesSearch);
                    }
                    #endregion
                    command.CommandText = query;

                    int n = (int)command.ExecuteScalar();
                    if (n > 0)
                    {
                        return true;
                    }
                    return false;

                } catch (Exception ex)
                {
                    throw new DriverRepositoryException("Exists failed" + ex.Message);
                } finally
                {
                    connection.Close();
                }
            }
        }

        //Done
        public void InsertDriver(string firstName, string lastName, string dateOfBirth, string nationalIdentificationNumber, List<LicenseType> licenses
            , int? addressId, int? fuelcardId, int? carId)
        {
            #region Check if given variables aren't empty or invalid
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new DriverRepositoryException("InsertDriver - FirstName cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new DriverRepositoryException("InsertDriver - LastName cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(dateOfBirth))
            {
                throw new DriverRepositoryException("InsertDriver - DateOfBirth cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(nationalIdentificationNumber))
            {
                throw new DriverRepositoryException("InsertDriver - NationalIdentificationNumber cannot be empty");
            }
            if (!ninValidator.isValid(nationalIdentificationNumber))
            {
                throw new DriverRepositoryException("InsertDriver - NationalIdentificationNumber is not valid");
            }
            #endregion

            #region Check if given variables aren't already in the database
            if (Exists(null, null, null, null, nationalIdentificationNumber, null))
            {
                throw new DriverRepositoryException("InsertDriver - Driver already exists in database");
            }
            #endregion

            SqlConnection connection = getConnection();
            string query = "INSERT INTO dbo.Driver ([FirstName], [LastName], [DateOfBirth], [NationalIdentificationNumber], [Licenses] ,[AddressId] ,[FuelcardId] ,[CarId])" +
                " VALUES (@FirstName, @LastName, @DateOfBirth, @NationalIdentificationNumber, @Licenses, @AddressId, @FuelcardId, @CarId)";

            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;

                    #region Add values to search terms
                    if (!string.IsNullOrWhiteSpace(firstName)) command.Parameters.AddWithValue("@FirstName", firstName);
                    if (!string.IsNullOrWhiteSpace(lastName)) command.Parameters.AddWithValue("@LastName", lastName);
                    if (!string.IsNullOrWhiteSpace(dateOfBirth)) command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    if (!string.IsNullOrWhiteSpace(nationalIdentificationNumber)) command.Parameters.AddWithValue("@NationalIdentificationNumber", nationalIdentificationNumber);
                    string licensesSearch = "";
                    bool first = true;
                    if (licenses == null)
                    {
                        command.Parameters.AddWithValue("@Licenses", DBNull.Value);
                    } else if (licenses != null)
                    {
                        foreach (var l in licenses)
                        {
                            if (l == LicenseType.A)
                            {
                                if (first) { licensesSearch += "A"; first = false; } else { licensesSearch += ", A"; }
                            }
                            if (l == LicenseType.A1)
                            {
                                if (first) { licensesSearch += "A1"; first = false; } else { licensesSearch += ", A1"; }
                            }
                            if (l == LicenseType.A2)
                            {
                                if (first) { licensesSearch += "A2"; first = false; } else { licensesSearch += ", A2"; }
                            }
                            if (l == LicenseType.AM)
                            {
                                if (first) { licensesSearch += "AM"; first = false; } else { licensesSearch += ", AM"; }
                            }
                            if (l == LicenseType.B)
                            {
                                if (first) { licensesSearch += "B"; first = false; } else { licensesSearch += ", B"; }
                            }
                            if (l == LicenseType.BE)
                            {
                                if (first) { licensesSearch += "BE"; first = false; } else { licensesSearch += ", BE"; }
                            }
                            if (l == LicenseType.C)
                            {
                                if (first) { licensesSearch += "C"; first = false; } else { licensesSearch += ", C"; }
                            }
                            if (l == LicenseType.CE)
                            {
                                if (first) { licensesSearch += "CE"; first = false; } else { licensesSearch += ", CE"; }
                            }
                            if (l == LicenseType.C1)
                            {
                                if (first) { licensesSearch += "C1"; first = false; } else { licensesSearch += ", C1"; }
                            }
                            if (l == LicenseType.C1E)
                            {
                                if (first) { licensesSearch += "C1E"; first = false; } else { licensesSearch += ", C1E"; }
                            }
                            if (l == LicenseType.D)
                            {
                                if (first) { licensesSearch += "D"; first = false; } else { licensesSearch += ", D"; }
                            }
                            if (l == LicenseType.DE)
                            {
                                if (first) { licensesSearch += "DE"; first = false; } else { licensesSearch += ", DE"; }
                            }
                            if (l == LicenseType.D1)
                            {
                                if (first) { licensesSearch += "D1"; first = false; } else { licensesSearch += ", D1"; }
                            }
                            if (l == LicenseType.D1E)
                            {
                                if (first) { licensesSearch += "D1E"; first = false; } else { licensesSearch += ", D1E"; }
                            }
                            if (l == LicenseType.G)
                            {
                                if (first) { licensesSearch += "G"; first = false; } else { licensesSearch += ", G"; }
                            }
                        }

                        command.Parameters.AddWithValue("@Licenses", licensesSearch);
                    }
                    if (addressId != null) command.Parameters.AddWithValue("@AddressId", addressId);
                    if (fuelcardId != null) command.Parameters.AddWithValue("@FuelcardId", fuelcardId);
                    if (carId != null) command.Parameters.AddWithValue("@CarId", carId);
                    #endregion

                    command.ExecuteNonQuery();

                } catch (Exception ex)
                {
                    throw new DriverRepositoryException("InsertDriver failed - " + ex.Message);
                } finally
                {
                    connection.Close();
                }
            }
        }

        //Done
        public void UpdateDriver(Driver oldDriverInfo, Driver newDriverInfo)
        {
            bool Comma = false;

            #region Check if there is a difference with the old driver (The old driver is already retrieved from the database)
            bool firstNameDifferent = oldDriverInfo.FirstName != newDriverInfo.FirstName;
            bool lastNameDifferent = oldDriverInfo.LastName != newDriverInfo.LastName;
            bool addressDifferent = oldDriverInfo.Address != newDriverInfo.Address;
            bool licensesDifferent = oldDriverInfo.Licenses != newDriverInfo.Licenses;
            bool carDifferent = oldDriverInfo.AssignedCar != newDriverInfo.AssignedCar;
            bool fuelcardDifferent = oldDriverInfo.AssignedFuelcard != newDriverInfo.AssignedFuelcard;
            #endregion

            if (firstNameDifferent || lastNameDifferent || addressDifferent || licensesDifferent || carDifferent || fuelcardDifferent)
            {
                SqlConnection connection = getConnection();
                string query = "UPDATE dbo.Driver SET ";

                #region For each thing different add it into the query and at the end add the where
                if (firstNameDifferent)
                {
                    query += "[FirstName] = @Cardnumber ";
                    Comma = true;
                }
                if (lastNameDifferent)
                {
                    if (Comma == true)
                    {
                        query += ",[LastName] = @LastName ";
                    } else
                    {
                        query += "[LastName] = @LastName ";
                        Comma = true;
                    }
                }
                if (licensesDifferent)
                {
                    if (Comma == true)
                    {
                        query += ",[Licenses] = @Licenses ";
                    } else
                    {
                        query += "[Licenses] = @Licenses ";
                        Comma = true;
                    }
                }
                if (addressDifferent)
                {
                    if (Comma == true)
                    {
                        query += ",[AddressId] = @AddressId ";
                    } else
                    {
                        query += "[AddressId] = @AddressId ";
                        Comma = true;
                    }
                }
                if (carDifferent)
                {
                    if (Comma == true)
                    {
                        query += ",[CarId] = @CarId ";
                    } else
                    {
                        query += "[CarId] = @CarId ";
                        Comma = true;
                    }
                }
                if (fuelcardDifferent)
                {
                    if (Comma == true)
                    {
                        query += ",[FuelcardId] = @FuelcardId ";
                    } else
                    {
                        query += "[FuelcardId] = @FuelcardId ";
                    }
                }

                query += "WHERE [DriverId] = @DriverId";
                #endregion

                using (SqlCommand command = connection.CreateCommand())
                {
                    try
                    {
                        connection.Open();
                        command.CommandText = query;

                        #region Add values of the new address if there is a difference
                        if (firstNameDifferent) command.Parameters.AddWithValue("@FirstName", newDriverInfo.FirstName);
                        if (lastNameDifferent) command.Parameters.AddWithValue("@LastName", newDriverInfo.LastName);
                        if (addressDifferent) command.Parameters.AddWithValue("@AddressId", newDriverInfo.Address.AddressId);
                        if (licensesDifferent)
                        {
                            if (newDriverInfo.Licenses == null)
                            {
                                command.Parameters.AddWithValue("@Licenses", DBNull.Value);
                            } else
                            {
                                string licensesString = "";
                                bool first = true;
                                foreach (var l in newDriverInfo.Licenses)
                                {
                                    if (first) { licensesString += licensesString += l.ToString(); ; first = false; } else { licensesString += ", " + l.ToString(); }
                                }

                                command.Parameters.AddWithValue("@Licenses", licensesString);
                            }
                        }
                        if (carDifferent)
                        {
                            if (newDriverInfo.AssignedCar == null)
                            {
                                command.Parameters.AddWithValue("@CarId", DBNull.Value);
                            } else
                            {
                                command.Parameters.AddWithValue("@CarId", newDriverInfo.AssignedCar.CarId);
                            }                           
                        }
                        if (fuelcardDifferent)
                        {
                            if (newDriverInfo.AssignedFuelcard == null)
                            {
                                command.Parameters.AddWithValue("@FuelcardId", DBNull.Value);
                            } else
                            {
                                command.Parameters.AddWithValue("@FuelcardId", newDriverInfo.AssignedFuelcard.FuelcardId);
                            }
                        }
                        #endregion

                        #region Add values of the old address in the where
                        command.Parameters.AddWithValue("@DriverId", oldDriverInfo.DriverId);
                        #endregion

                        command.ExecuteNonQuery();


                    } catch (Exception ex)
                    {
                        throw new AddressRepositoryException("UpdateDriver failed - " + ex.Message);
                    } finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        //Done
        public void DeleteDriver(Driver driver)
        {
            SqlConnection connection = getConnection();
            string query = "Delete FROM [dbo].[Driver] WHERE DriverId = @Id AND NationalIdentificationNumber = @NationalIdentificationNumber";

            using SqlCommand command = new(query, connection);
            try
            {
                connection.Open();
                #region Add values to the search terms
                command.Parameters.AddWithValue("@Id", driver.DriverId);
                command.Parameters.AddWithValue("@NationalIdentificationNumber", driver.NationalIdentificationNumber);
                #endregion

                command.ExecuteNonQuery();

            } catch (Exception ex)
            {
                throw new AddressRepositoryException("DeleteDriver failed - " + ex.Message);
            } finally
            {
                connection.Close();
            }
        }
    }
}
