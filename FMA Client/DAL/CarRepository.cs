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
    public class CarRepository : ICarRepository
    {
        private string _connectionString;
        private VINValidator vinValidator = new();
        private LicenseplateValidator licenseplateValidator = new();

        public CarRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            return connection;
        }

        //Done -> Driver kunnen bijverkrijgen 
        public IReadOnlyList<Car> GetAllCars()
        {
            List<Car> cars = new();
            SqlConnection connection = getConnection();
            string query = "SELECT *, C.Fueltypes AS carFueltypes, F.Fueltypes AS fuelcardFueltypes FROM dbo.Car AS C" +
                " LEFT JOIN dbo.Driver AS D ON C.CarId = D.CarId" +
                " LEFT JOIN dbo.Address AS A ON D.AddressId = A.AddressId" +
                " LEFT JOIN dbo.Fuelcard AS F ON D.FuelcardId = F.FuelcardId";

            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    SqlDataReader datareader = command.ExecuteReader();
                    while (datareader.Read())
                    {
                        int carId = (int)datareader["CarId"];
                        string make = (string)datareader["Make"];
                        string model = (string)datareader["Model"];
                        string vin = (string)datareader["Vin"];
                        string licenseplate = (string)datareader["Licenseplate"];
                        string vehicleType = (string)datareader["VehicleType"];
                        string fueltypesString = (string)datareader["Fueltypes"];
                        string doors = "";
                        string colour = "";
                        Driver driver = null;
                        #region Null checking
                        if (DBNull.Value != datareader["Doors"])
                        {
                            doors = (string)datareader["Doors"];
                        }
                        if (DBNull.Value != datareader["Colour"])
                        {
                            colour = (string)datareader["Colour"];
                        }
                        if (DBNull.Value != datareader["DriverId"])
                        {
                            int driverId = (int)datareader["DriverId"];
                            string firstName = (string)datareader["FirstName"];
                            string lastName = (string)datareader["LastName"];
                            DateTime dateOfBirth = (DateTime)datareader["DateOfBirth"];
                            string nationalIdentificationNumber = (string)datareader["NationalIdentificationNumber"];
                            List<LicenseType> licensesList = new();
                            Address address = null;
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
                                int postalcode = (int)datareader["Postalcode"];
                                string city = (string)datareader["City"];
                                string addendum = "";

                                #region Null checking
                                if (DBNull.Value != datareader["Addendum"])
                                {
                                    addendum = (string)datareader["Addendum"];
                                }

                                address = new(addressId, street, housenumber, addendum, city, postalcode, null);
                            }
                            if (DBNull.Value != datareader["FuelcardId"])
                            {

                                int fuelcardId = (int)datareader["FuelcardId"];
                                string cardnumber = (string)datareader["Cardnumber"];
                                DateTime expiryDate = (DateTime)datareader["ExpiryDate"];
                                bool isActive = (bool)datareader["IsActive"];
                                List<Fuel> fueltypesListFuelcard = new();

                                if (DBNull.Value != datareader["fuelcardFueltypes"])
                                {
                                    #region Db fueltype to program
                                    //In the database the fueltypes are put in as a string with a "," seperating each type.
                                    //This basically turns it into a list for us to use
                                    //We're also making the fueltypes all upper case in the array to eliminate the case sensitivity
                                    string fueltypesStringFuelcard = (string)datareader["fuelcardFueltypes"];
                                    fueltypesStringFuelcard = fueltypesStringFuelcard.Replace(" ", "");
                                    string[] fueltypesArrayFuelcard = fueltypesStringFuelcard.ToUpper().Split(",");

                                    foreach (var f in fueltypesArrayFuelcard)
                                    {
                                        if (string.IsNullOrWhiteSpace(f)) { continue; }
                                        if (f == Fuel.Benzine.ToString().ToUpper())
                                        {
                                            fueltypesListFuelcard.Add(Fuel.Benzine);
                                        }
                                        if (f == Fuel.Diesel.ToString().ToUpper())
                                        {
                                            fueltypesListFuelcard.Add(Fuel.Diesel);
                                        }
                                        if (f == Fuel.Electrisch.ToString().ToUpper())
                                        {
                                            fueltypesListFuelcard.Add(Fuel.Electrisch);
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

                                fuelcard = new(fuelcardId, cardnumber, expiryDate, /*pincode,*/ fueltypesListFuelcard, isActive);
                            }
                            #endregion

                            driver = new(driverId, firstName, lastName, address, dateOfBirth, nationalIdentificationNumber, licensesList, null, fuelcard);
                        }
                        #endregion

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
                        #endregion

                        Car car = new(carId, make, model, vin, licenseplate, vehicleType, fueltypesList, driver, colour, doors);

                        cars.Add(car);
                    }
                    datareader.Close();

                } catch (Exception ex)
                {
                    throw new CarRepositoryException("GetAllCars failed" + ex.Message);
                } finally
                {
                    connection.Close();
                }

                return cars;
            }
        }

        //Done
        public IReadOnlyList<Car> GetCars(int? carId, string vin, string licenseplate, string make, string model, string vehicleType
            , List<Fuel> fueltypes, string doors, string colour)
        {
            bool AND = false;
            List<Car> cars = new();
            SqlConnection connection = getConnection();
            string query = "SELECT *, C.Fueltypes AS carFueltypes, F.Fueltypes AS fuelcardFueltypes FROM dbo.Car AS C" +
                " LEFT JOIN dbo.Driver AS D ON C.CarId = D.CarId" +
                " LEFT JOIN dbo.Address AS A ON D.AddressId = A.AddressId" +
                " LEFT JOIN dbo.Fuelcard AS F ON D.FuelcardId = F.FuelcardId ";

            #region Add to the query for search function
            if (carId != null)
            {
                query += "WHERE C.CarId = @CarId ";
                AND = true;
            }
            if (!string.IsNullOrWhiteSpace(vin))
            {
                if (AND == true)
                {
                    query += "AND C.Vin = @Vin ";
                } else
                {
                    query += "WHERE C.Vin = @Vin ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(licenseplate))
            {
                if (AND == true)
                {
                    query += "AND C.Licenseplate = @Licenseplate ";
                } else
                {
                    query += "WHERE C.Licenseplate = @Licenseplate ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(make))
            {
                if (AND == true)
                {
                    query += "AND C.Make = @Make ";
                } else
                {
                    query += "WHERE C.Make = @Make ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(model))
            {
                if (AND == true)
                {
                    query += "AND C.Model = @Model ";
                } else
                {
                    query += "WHERE C.Model = @Model ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(vehicleType))
            {
                if (AND == true)
                {
                    query += "AND C.VehicleType = @VehicleType ";
                } else
                {
                    query += "WHERE C.VehicleType = @VehicleType ";
                    AND = true;
                }
            }
            if (fueltypes != null)
            {
                if (AND == true)
                {
                    query += "AND C.Fueltypes = @Fueltypes ";
                } else
                {
                    query += "WHERE C.Fueltypes = @Fueltypes ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(doors))
            {
                if (AND == true)
                {
                    query += "AND C.Doors = @Doors ";
                } else
                {
                    query += "WHERE C.Doors = @Doors ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(colour))
            {
                if (AND == true)
                {
                    query += "AND C.Colour = @Colour ";
                } else
                {
                    query += "WHERE C.Colour = @Colour ";
                    AND = true;
                }
            }
            #endregion

            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    #region Add values to the search terms
                    if (carId != null) command.Parameters.AddWithValue("@CarId", carId);
                    if (!string.IsNullOrWhiteSpace(vin)) command.Parameters.AddWithValue("@Vin", vin);
                    if (!string.IsNullOrWhiteSpace(licenseplate)) command.Parameters.AddWithValue("@Licenseplate", licenseplate);
                    if (!string.IsNullOrWhiteSpace(make)) command.Parameters.AddWithValue("@Make", make);
                    if (!string.IsNullOrWhiteSpace(model)) command.Parameters.AddWithValue("@Model", model);
                    if (!string.IsNullOrWhiteSpace(vehicleType)) command.Parameters.AddWithValue("@VehicleType", vehicleType);
                    if (!string.IsNullOrWhiteSpace(doors)) command.Parameters.AddWithValue("@Doors", doors);
                    if (!string.IsNullOrWhiteSpace(colour)) command.Parameters.AddWithValue("@Colour", colour);
                    if (fueltypes != null)
                    {
                        //Here we need to transform the fueltype from an enum to a string seeing as that's how it's stored in the Db
                        string fueltypesString = "";
                        bool first = true;

                        foreach (var f in fueltypes)
                        {
                            if (f == Fuel.Benzine)
                            {
                                if (first) { fueltypesString += "Benzine"; first = false; } else { fueltypesString += ", Benzine"; }

                            }
                            if (f == Fuel.Diesel)
                            {
                                if (first) { fueltypesString += "Diesel"; first = false; } else { fueltypesString += ", Diesel"; }
                            }
                            if (f == Fuel.Electrisch)
                            {
                                if (first) { fueltypesString += "Electrisch"; first = false; } else { fueltypesString += ", Electrisch"; }
                            }
                        }

                        command.Parameters.AddWithValue("@Fueltypes", fueltypesString);
                    }
                    #endregion
                    command.CommandText = query;
                    SqlDataReader datareader = command.ExecuteReader();
                    while (datareader.Read())
                    {
                        int carIdDb = (int)datareader["CarId"];
                        string makeDb = (string)datareader["Make"];
                        string modelDb = (string)datareader["Model"];
                        string vinDb = (string)datareader["Vin"];
                        string licenseplateDb = (string)datareader["Licenseplate"];
                        string vehicleTypeDb = (string)datareader["VehicleType"];
                        string fueltypesStringDb = (string)datareader["Fueltypes"];
                        string doorsDb = "";
                        string colourDb = "";
                        Driver driver = null;
                        #region Null checking
                        if (DBNull.Value != datareader["Doors"])
                        {
                            doors = (string)datareader["Doors"];
                        }
                        if (DBNull.Value != datareader["Colour"])
                        {
                            colour = (string)datareader["Colour"];
                        }
                        if (DBNull.Value != datareader["DriverId"])
                        {
                            int driverId = (int)datareader["DriverId"];
                            string firstName = (string)datareader["FirstName"];
                            string lastName = (string)datareader["LastName"];
                            DateTime dateOfBirth = (DateTime)datareader["DateOfBirth"];
                            string nationalIdentificationNumber = (string)datareader["NationalIdentificationNumber"];
                            List<LicenseType> licensesList = new();
                            Address address = null;
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
                                int postalcode = (int)datareader["Postalcode"];
                                string city = (string)datareader["City"];
                                string addendum = "";

                                #region Null checking
                                if (DBNull.Value != datareader["Addendum"])
                                {
                                    addendum = (string)datareader["Addendum"];
                                }

                                address = new(addressId, street, housenumber, addendum, city, postalcode, null);
                            }
                            if (DBNull.Value != datareader["FuelcardId"])
                            {

                                int fuelcardId = (int)datareader["FuelcardId"];
                                string cardnumber = (string)datareader["Cardnumber"];
                                DateTime expiryDate = (DateTime)datareader["ExpiryDate"];
                                bool isActive = (bool)datareader["IsActive"];
                                List<Fuel> fueltypesListFuelcard = new();

                                if (DBNull.Value != datareader["fuelcardFueltypes"])
                                {
                                    #region Db fueltype to program
                                    //In the database the fueltypes are put in as a string with a "," seperating each type.
                                    //This basically turns it into a list for us to use
                                    //We're also making the fueltypes all upper case in the array to eliminate the case sensitivity
                                    string fueltypesStringFuelcard = (string)datareader["fuelcardFueltypes"];
                                    fueltypesStringFuelcard = fueltypesStringFuelcard.Replace(" ", "");
                                    string[] fueltypesArrayFuelcard = fueltypesStringFuelcard.ToUpper().Split(",");

                                    foreach (var f in fueltypesArrayFuelcard)
                                    {
                                        if (string.IsNullOrWhiteSpace(f)) { continue; }
                                        if (f == Fuel.Benzine.ToString().ToUpper())
                                        {
                                            fueltypesListFuelcard.Add(Fuel.Benzine);
                                        }
                                        if (f == Fuel.Diesel.ToString().ToUpper())
                                        {
                                            fueltypesListFuelcard.Add(Fuel.Diesel);
                                        }
                                        if (f == Fuel.Electrisch.ToString().ToUpper())
                                        {
                                            fueltypesListFuelcard.Add(Fuel.Electrisch);
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

                                fuelcard = new(fuelcardId, cardnumber, expiryDate, /*pincode,*/ fueltypesListFuelcard, isActive);
                            }
                            #endregion

                            driver = new(driverId, firstName, lastName, address, dateOfBirth, nationalIdentificationNumber, licensesList, null, fuelcard);
                        }
                        #endregion

                        #region Fueltypes from string (db) to a list
                        List<Fuel> fueltypesList = new();
                        fueltypesStringDb = fueltypesStringDb.Replace(" ", "");
                        string[] fueltypesArray = fueltypesStringDb.ToUpper().Split(",");

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
                        #endregion

                        Car car = new(carIdDb, makeDb, modelDb, vinDb, licenseplateDb, vehicleTypeDb, fueltypesList, driver, colourDb, doorsDb);

                        cars.Add(car);
                    }
                    datareader.Close();

                } catch (Exception ex)
                {
                    throw new CarRepositoryException("GetCars failed" + ex.Message);
                } finally
                {
                    connection.Close();
                }

                return cars;
            }
        }

        //Done
        public bool Exists(int? carId, string vin, string licenseplate, string make, string model, string vehicleType
            , List<Fuel> fueltypes, string doors, string colour)
        {
            bool AND = false;
            List<Car> cars = new();
            SqlConnection connection = getConnection();
            string query = "SELECT Count(*) FROM dbo.Car ";

            #region Add to the query for search function
            if (carId != null)
            {
                query += "WHERE CarId = @CarId ";
                AND = true;
            }
            if (!string.IsNullOrWhiteSpace(vin))
            {
                if (AND == true)
                {
                    query += "AND Vin = @Vin ";
                } else
                {
                    query += "WHERE Vin = @Vin ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(licenseplate))
            {
                if (AND == true)
                {
                    query += "AND Licenseplate = @Licenseplate ";
                } else
                {
                    query += "WHERE Licenseplate = @Licenseplate ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(make))
            {
                if (AND == true)
                {
                    query += "AND Make = @Make ";
                } else
                {
                    query += "WHERE Make = @Make ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(model))
            {
                if (AND == true)
                {
                    query += "AND Model = @Model ";
                } else
                {
                    query += "WHERE Model = @Model ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(vehicleType))
            {
                if (AND == true)
                {
                    query += "AND VehicleType = @VehicleType ";
                } else
                {
                    query += "WHERE VehicleType = @VehicleType ";
                    AND = true;
                }
            }
            if (fueltypes != null)
            {
                if (AND == true)
                {
                    query += "AND Fueltypes = @Fueltypes ";
                } else
                {
                    query += "WHERE Fueltypes = @Fueltypes ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(doors))
            {
                if (AND == true)
                {
                    query += "AND Doors = @Doors ";
                } else
                {
                    query += "WHERE Doors = @Doors ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(colour))
            {
                if (AND == true)
                {
                    query += "AND Colour = @Colour ";
                } else
                {
                    query += "WHERE Colour = @Colour ";
                    AND = true;
                }
            }
            #endregion

            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    #region Add values to the search terms
                    if (carId != null) command.Parameters.AddWithValue("@CarId", carId);
                    if (!string.IsNullOrWhiteSpace(vin)) command.Parameters.AddWithValue("@Vin", vin);
                    if (!string.IsNullOrWhiteSpace(licenseplate)) command.Parameters.AddWithValue("@Licenseplate", licenseplate);
                    if (!string.IsNullOrWhiteSpace(make)) command.Parameters.AddWithValue("@Make", make);
                    if (!string.IsNullOrWhiteSpace(model)) command.Parameters.AddWithValue("@Model", model);
                    if (!string.IsNullOrWhiteSpace(vehicleType)) command.Parameters.AddWithValue("@VehicleType", vehicleType);
                    if (!string.IsNullOrWhiteSpace(doors)) command.Parameters.AddWithValue("@Doors", doors);
                    if (!string.IsNullOrWhiteSpace(colour)) command.Parameters.AddWithValue("@Colour", colour);
                    if (fueltypes != null)
                    {
                        //Here we need to transform the fueltype from an enum to a string seeing as that's how it's stored in the Db
                        string fueltypesString = "";
                        bool first = true;

                        foreach (var f in fueltypes)
                        {
                            if (f == Fuel.Benzine)
                            {
                                if (first) { fueltypesString += "Benzine"; first = false; } else { fueltypesString += ", Benzine"; }

                            }
                            if (f == Fuel.Diesel)
                            {
                                if (first) { fueltypesString += "Diesel"; first = false; } else { fueltypesString += ", Diesel"; }
                            }
                            if (f == Fuel.Electrisch)
                            {
                                if (first) { fueltypesString += "Electrisch"; first = false; } else { fueltypesString += ", Electrisch"; }
                            }
                        }

                        command.Parameters.AddWithValue("@Fueltypes", fueltypesString);
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
                    throw new CarRepositoryException("Exists failed" + ex.Message);
                } finally
                {
                    connection.Close();
                }
            }
        }
        
        //Done
        public void InsertCar(string vin, string licenseplate, string make, string model, string vehicleType
            , List<Fuel> fueltypes, string doors, string colour)
        {
            #region Check if given variables aren't empty
            if (string.IsNullOrWhiteSpace(vin))
            {
                throw new CarRepositoryException("InsertCar - Vin cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(licenseplate))
            {
                throw new CarRepositoryException("InsertCar - Licenseplate cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(make))
            {
                throw new CarRepositoryException("InsertCar - Make cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(model))
            {
                throw new CarRepositoryException("InsertCar - Model cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(vehicleType))
            {
                throw new CarRepositoryException("InsertCar - VehicleType cannot be empty");
            }
            if (fueltypes == null)
            {
                throw new CarRepositoryException("InsertCar - Fueltypes cannot be empty");
            }
            #endregion

            #region Check if given variables aren't invalid
            if (!vinValidator.IsValid(vin))
            {
                throw new CarRepositoryException("InsertCar - Vin is not valid");
            }
            if (!licenseplateValidator.isValid(licenseplate))
            {
                throw new CarRepositoryException("InsertCar - Licenseplate is not valid");
            }
            #endregion

            #region Check if given variables aren't already in the database
            if (Exists(null, vin, licenseplate, null, null, null, null, null, null))
            {
                throw new CarRepositoryException("InsertCar - Car already exists in database");
            }
            #endregion

            SqlConnection connection = getConnection();
            string query = "INSERT INTO dbo.Car ([Vin],[Licenseplate],[Make],[Model],[VehicleType],[Fueltypes],[Doors],[Colour])" +
                " VALUES (@Vin, @Licenseplate, @Make, @Model, @VehicleType, @Fueltypes, @Doors, @Colour)";

            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@Vin", vin);
                    command.Parameters.AddWithValue("@Licenseplate", licenseplate);
                    command.Parameters.AddWithValue("@Make", make);
                    command.Parameters.AddWithValue("@Model", model);
                    command.Parameters.AddWithValue("@VehicleType", vehicleType);

                    #region Null Checking
                    if (fueltypes != null)
                    {
                        string fueltypesString = "";
                        bool first = true;

                        foreach (var f in fueltypes)
                        {
                            if (f == Fuel.Benzine)
                            {
                                if (first) { fueltypesString += "Benzine"; first = false; } else { fueltypesString += ", Benzine"; }

                            }
                            if (f == Fuel.Diesel)
                            {
                                if (first) { fueltypesString += "Diesel"; first = false; } else { fueltypesString += ", Diesel"; }
                            }
                            if (f == Fuel.Electrisch)
                            {
                                if (first) { fueltypesString += "Electrisch"; first = false; } else { fueltypesString += ", Electrisch"; }
                            }
                        }

                        command.Parameters.AddWithValue("@Fueltypes", fueltypesString);
                    } else
                    {
                        command.Parameters.AddWithValue("@Fueltypes", DBNull.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(doors))
                    {
                        command.Parameters.AddWithValue("@Doors", doors);
                    } else
                    {
                        command.Parameters.AddWithValue("@Doors", DBNull.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(colour))
                    {
                        command.Parameters.AddWithValue("@Colour", colour);
                    } else
                    {
                        command.Parameters.AddWithValue("@Colour", DBNull.Value);
                    }
                    #endregion

                    command.ExecuteNonQuery();

                } catch (Exception ex)
                {
                    throw new CarRepositoryException("InsertCar failed - " + ex.Message);
                } finally
                {
                    connection.Close();
                }
            }
        }

        //Done -> Only able to change lincenseplate and colour
        public void UpdateCar(Car oldCarInfo, Car newCarInfo)
        {
            bool Comma = false;

            #region Check if there is a difference with the old address (The old address is already retrieved from the database
            bool licenseplateDifferent = oldCarInfo.Licenseplate != newCarInfo.Licenseplate;
            bool colourDifferent = oldCarInfo.Colour != newCarInfo.Colour;
            #endregion

            if (licenseplateDifferent || colourDifferent)
            {
                SqlConnection connection = getConnection();
                string query = "UPDATE dbo.Car SET ";

                #region For each thing different add it into the query and at the end add the where
                if (licenseplateDifferent)
                {
                    query += "[Licenseplate] = @Licenseplate ";
                    Comma = true;
                }
                if (colourDifferent)
                {
                    if (Comma == true)
                    {
                        query += ",[Colour] = @Colour ";
                    } else
                    {
                        query += "[Colour] = @Colour ";
                        Comma = true;
                    }
                }

                query += "WHERE [CarId] = @OldCarId AND [Vin] = @OldCarVin";
                #endregion

                using (SqlCommand command = connection.CreateCommand())
                {
                    try
                    {
                        connection.Open();
                        command.CommandText = query;

                        #region Add values of the new address if there is a difference
                        if (licenseplateDifferent) command.Parameters.AddWithValue("@Licenseplate", newCarInfo.Licenseplate);
                        if (colourDifferent) command.Parameters.AddWithValue("@Colour", newCarInfo.Colour);

                        #endregion
                        #region Add values of the old address in the where
                        command.Parameters.AddWithValue("@OldCarId", oldCarInfo.CarId);
                        command.Parameters.AddWithValue("@OldCarVin", oldCarInfo.Vin);
                        #endregion

                        command.ExecuteNonQuery();


                    } catch (Exception ex)
                    {
                        throw new AddressRepositoryException("UpdateCar failed - " + ex.Message);
                    } finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        //Done
        public void DeleteCar(Car car)
        {
            //Only need to delete based on Carid actually but we can use the Vin as a sort of protection
            SqlConnection connection = getConnection();
            string query = "Delete FROM [dbo].[Car] WHERE CarId = @Id AND Vin = @Vin";

            using SqlCommand command = new(query, connection);
            try
            {
                connection.Open();
                #region Add values to the search terms
                command.Parameters.AddWithValue("@Id", car.CarId);
                command.Parameters.AddWithValue("@Vin", car.Vin);
                #endregion

                command.ExecuteNonQuery();

            } catch (Exception ex)
            {
                throw new AddressRepositoryException("DeleteCar failed - " + ex.Message);
            } finally
            {
                connection.Close();
            }
        }
    }
}
