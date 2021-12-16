using BusinessLayer;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class AddressRepository : IAddressRepository
    {
        private string _connectionString;

        public AddressRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            return connection;
        }

        //Done
        public IReadOnlyList<Address> GetAllAddresses()
        {
            List<Address> addresses = new();
            SqlConnection connection = getConnection();
            string query = "SELECT *, C.Fueltypes AS carFueltypes, F.Fueltypes AS fuelcardFueltypes FROM dbo.Address AS A" +
                " LEFT JOIN dbo.Driver AS D ON A.AddressId = D.AddressId" +
                " LEFT JOIN dbo.Car AS C ON D.CarId = C.CarId" +
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
                        int addressId = (int)datareader["AddressId"];
                        string street = (string)datareader["Street"];
                        string housenumber = (string)datareader["Housenumber"];
                        int postalcode = (int)datareader["Postalcode"];
                        string city = (string)datareader["City"];
                        string addendum = "";
                        Driver driver = null;
                        
                        #region Null checking
                        if (DBNull.Value != datareader["Addendum"])
                        {
                            addendum = (string)datareader["Addendum"];
                        }
                        if (DBNull.Value != datareader["DriverId"])
                        {
                            int driverId = (int)datareader["DriverId"];
                            string firstName = (string)datareader["FirstName"];
                            string lastName = (string)datareader["LastName"];
                            DateTime dateOfBirth = (DateTime)datareader["DateOfBirth"];
                            string nationalIdentificationNumber = (string)datareader["NationalIdentificationNumber"];
                            List<LicenseType> licensesList = new();
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

                            driver = new(driverId, firstName, lastName, null, dateOfBirth, nationalIdentificationNumber, licensesList, car, fuelcard);
                        }
                        #endregion

                        Address address = new(addressId, street, housenumber, addendum, city, postalcode, driver);

                        addresses.Add(address);
                    }
                    datareader.Close();

                } catch (Exception ex)
                {
                    throw new AddressRepositoryException("GetAllAddresses failed" + ex.Message);
                } finally
                {
                    connection.Close();
                }

                return addresses;
            }
        }

        //Done
        public IReadOnlyList<Address> GetAddress(int? id, string street, string housenumber, string addendum, string city, int? postalcode)
        {
            List<Address> addresses = new();
            bool AND = false;
            SqlConnection connection = getConnection();
            string query = "SELECT *, C.Fueltypes AS carFueltypes, F.Fueltypes AS fuelcardFueltypes FROM dbo.Address AS A" +
                 " LEFT JOIN dbo.Driver AS D ON A.AddressId = D.AddressId" +
                 " LEFT JOIN dbo.Car AS C ON D.CarId = C.CarId" +
                 " LEFT JOIN dbo.Fuelcard AS F ON D.FuelcardId = F.FuelcardId ";
            #region Add to the query for search function
            if (id != null)
            {
                query += "WHERE A.AddressId = @Id ";
                AND = true;
            }
            if (!string.IsNullOrWhiteSpace(street))
            {
                if (AND == true)
                {
                    query += "AND A.Street = @Street ";
                } else
                {
                    query += "WHERE A.Street = @Street ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(housenumber))
            {
                if (AND == true)
                {
                    query += "AND A.Housenumber = @Housenumber ";
                } else
                {
                    query += "WHERE A.Housenumber = @Housenumber ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(addendum))
            {
                if (AND == true)
                {
                    query += "AND A.Addendum = @Addendum ";
                } else
                {
                    query += "WHERE A.Addendum = @Addendum ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(city))
            {
                if (AND == true)
                {
                    query += "AND A.City = @City ";
                } else
                {
                    query += "WHERE A.City = @City ";
                    AND = true;
                }
            }
            if (postalcode != null)
            {
                if (AND == true)
                {
                    query += "AND A.Postalcode = @Postalcode ";
                } else
                {
                    query += "WHERE A.Postalcode = @Postalcode ";
                    AND = true;
                }
            }
            #endregion

            using SqlCommand command = new(query, connection);
            try
            {
                connection.Open();
                #region Add values to the search terms
                if (id != null)
                {
                    command.Parameters.AddWithValue("@Id", id);
                }
                if (!string.IsNullOrWhiteSpace(street))
                {
                    command.Parameters.AddWithValue("@Street", street);
                }
                if (!string.IsNullOrWhiteSpace(housenumber))
                {
                    command.Parameters.AddWithValue("@Housenumber", housenumber);
                }
                if (!string.IsNullOrWhiteSpace(addendum))
                {
                    command.Parameters.AddWithValue("@Addendum", addendum);
                }
                if (!string.IsNullOrWhiteSpace(city))
                {
                    command.Parameters.AddWithValue("@City", city);
                }
                if (postalcode != null)
                {
                    command.Parameters.AddWithValue("@Postalcode", postalcode);
                }
                #endregion

                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    #region Retrieve values from the database
                    int addressIdDb = (int)datareader["AddressId"];
                    string streetDb = (string)datareader["Street"];
                    string housenumberDb = (string)datareader["Housenumber"];
                    int postalcodeDb = (int)datareader["Postalcode"];
                    string cityDb = (string)datareader["City"];
                    string addendumDb = "";
                    Driver driver = null;

                    #region Null checking
                    if (DBNull.Value != datareader["Addendum"])
                    {
                        addendum = (string)datareader["Addendum"];
                    }
                    if (DBNull.Value != datareader["DriverId"])
                    {
                        int driverId = (int)datareader["DriverId"];
                        string firstName = (string)datareader["FirstName"];
                        string lastName = (string)datareader["LastName"];
                        DateTime dateOfBirth = (DateTime)datareader["DateOfBirth"];
                        string nationalIdentificationNumber = (string)datareader["NationalIdentificationNumber"];
                        List<LicenseType> licensesList = new();
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

                        driver = new(driverId, firstName, lastName, null, dateOfBirth, nationalIdentificationNumber, licensesList, car, fuelcard);
                    }
                    #endregion
                    #endregion
                    Address address = new(addressIdDb, streetDb, housenumberDb, addendumDb, cityDb, postalcodeDb, driver);

                    addresses.Add(address);
                }
                datareader.Close();
            } catch (Exception ex)
            {
                throw new AddressRepositoryException("GetAddress failed" + ex.Message);
            } finally
            {
                connection.Close();

            }
            return addresses;
        }

        //Done
        public bool Exists(int? id, string street, string housenumber, string addendum, string city, int? postalcode)
        {
            bool AND = false;
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM [dbo].[Address] WHERE ";
            #region Add to the query for search function
            if (id != null)
            {
                query += "AddressId = @Id ";
                AND = true;
            }
            if (!string.IsNullOrWhiteSpace(street))
            {
                if (AND == true)
                {
                    query += "AND Street = @Street ";
                } else
                {
                    query += "Street = @Street ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(housenumber))
            {
                if (AND == true)
                {
                    query += "AND Housenumber = @Housenumber ";
                } else
                {
                    query += "Housenumber = @Housenumber ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(addendum))
            {
                if (AND == true)
                {
                    query += "AND Addendum = @Addendum ";
                } else
                {
                    query += "Addendum = @Addendum ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(city))
            {
                if (AND == true)
                {
                    query += "AND City = @City ";
                } else
                {
                    query += "City = @City ";
                    AND = true;
                }
            }
            if (postalcode != null)
            {
                if (AND == true)
                {
                    query += "AND Postalcode = @Postalcode ";
                } else
                {
                    query += "Postalcode = @Postalcode ";
                    AND = true;
                }
            }
            #endregion

            using SqlCommand command = new(query, connection);
            try
            {
                connection.Open();
                #region Add values to the search terms
                if (id != null)
                {
                    command.Parameters.AddWithValue("@Id", id);
                }
                if (!string.IsNullOrWhiteSpace(street))
                {
                    command.Parameters.AddWithValue("@Street", street);
                }
                if (!string.IsNullOrWhiteSpace(housenumber))
                {
                    command.Parameters.AddWithValue("@Housenumber", housenumber);
                }
                if (!string.IsNullOrWhiteSpace(addendum))
                {
                    command.Parameters.AddWithValue("@Addendum", addendum);
                }
                if (!string.IsNullOrWhiteSpace(city))
                {
                    command.Parameters.AddWithValue("@City", city);
                }
                if (postalcode != null)
                {
                    command.Parameters.AddWithValue("@Postalcode", postalcode);
                }
                #endregion

                int n = (int)command.ExecuteScalar();
                if (n > 0)
                {
                    return true;
                }
                return false;
            } catch (Exception ex)
            {
                throw new AddressRepositoryException("Exists failed" + ex.Message);
            } finally
            {
                connection.Close();
            }
        }

        //Done
        public void InsertAddress(string street, string housenumber, string addendum, string city, int postalcode)
        {
            #region Check if given variables aren't empty or invalid
            if (string.IsNullOrWhiteSpace(street))
            {
                throw new AddressRepositoryException("InsertDriver - Street cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(housenumber))
            {
                throw new AddressRepositoryException("InsertDriver - Housenumber cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new AddressRepositoryException("InsertDriver - City cannot be empty");
            }
            if (postalcode < 1000 || postalcode > 9992)
            {
                throw new AddressRepositoryException("InsertDriver - Postalcode is not a valid number");
            }
            #endregion

            //Check if given variables aren't already in the database
            if (Exists(null, street, housenumber, addendum, city, postalcode))
            {
                throw new AddressRepositoryException("InsertAddress - Address already exists in database");
            }

            SqlConnection connection = getConnection();
            string query = "INSERT INTO dbo.Address ([Street],[Housenumber],[Addendum],[City],[Postalcode]) VALUES (@Street, @Housenumber, @Addendum, @City, @Postalcode)";

            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@Street", street);
                    command.Parameters.AddWithValue("@Housenumber", housenumber);
                    if (!string.IsNullOrWhiteSpace(addendum))
                    {
                        command.Parameters.AddWithValue("@Addendum", addendum);
                    } else
                    {
                        command.Parameters.AddWithValue("@Addendum", DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@City", city);
                    command.Parameters.AddWithValue("@Postalcode", postalcode);

                    command.ExecuteNonQuery();

                } catch (Exception ex)
                {
                    throw new AddressRepositoryException("InsertAddress failed - " + ex.Message);
                } finally
                {
                    connection.Close();
                }
            }
        }

        //Done
        public void DeleteAddress(Address address)
        {
            //Answer: Not needed since you'll be deleting using the UI and this will be retrieved from the database (thus it can't be empty)
            //#region Check if at least street, housenumber and city has been given, is this actually needed?
            //if (string.IsNullOrWhiteSpace(address.Street)) throw new AddressRepositoryException("DeleteAddress - Street cannot be empty");
            //if (string.IsNullOrWhiteSpace(address.Housenumber)) throw new AddressRepositoryException("DeleteAddress - Housenumber cannot be empty");
            //if (string.IsNullOrWhiteSpace(address.City)) throw new AddressRepositoryException("DeleteAddress - City cannot be empty");
            //#endregion

            SqlConnection connection = getConnection();
            string query = "Delete FROM [dbo].[Address] WHERE AddressId = @Id AND Street = @Street AND" +
                " Housenumber = @Housenumber AND City = @City AND Postalcode = @Postalcode";

            if (!string.IsNullOrWhiteSpace(address.Addendum)) query += " AND Addendum = @Addendum";

            using SqlCommand command = new(query, connection);
            try
            {
                connection.Open();
                #region Add values to the search terms
                command.Parameters.AddWithValue("@Id", address.AddressId);
                command.Parameters.AddWithValue("@Street", address.Street);
                command.Parameters.AddWithValue("@Housenumber", address.Housenumber);
                command.Parameters.AddWithValue("@City", address.City);
                command.Parameters.AddWithValue("@Postalcode", address.Postalcode);
                if (!string.IsNullOrWhiteSpace(address.Addendum))
                {
                    command.Parameters.AddWithValue("@Addendum", address.Addendum);
                }
                #endregion

                command.ExecuteNonQuery();

            } catch (Exception ex)
            {
                throw new AddressRepositoryException("DeleteAddress failed - " + ex.Message);
            } finally
            {
                connection.Close();
            }
        }

        //Done
        public void UpdateAddress(Address oldAddressInfo, Address newAddressInfo)
        {
            bool Comma = false;

            #region Check if there is a difference with the old address (The old address is already retrieved from the database
            bool StreetDifferent = oldAddressInfo.Street.ToUpper() != newAddressInfo.Street.ToUpper();
            bool HousenumberDifferent = oldAddressInfo.Housenumber.ToUpper() != newAddressInfo.Housenumber.ToUpper();
            bool AddendumDifferent = oldAddressInfo.Addendum != newAddressInfo.Addendum;
            bool CityDifferent = oldAddressInfo.City.ToUpper() != newAddressInfo.City.ToUpper();
            bool PostalcodeDifferent = oldAddressInfo.Postalcode != newAddressInfo.Postalcode;
            #endregion
            if(StreetDifferent || HousenumberDifferent || AddendumDifferent || CityDifferent || PostalcodeDifferent)
            {
                SqlConnection connection = getConnection();
                string query = "UPDATE dbo.Address SET ";

                #region For each thing different add it into the query and at the end add the where
                if (StreetDifferent)
                {
                    query += "[Street] = @Street ";
                    Comma = true;
                }
                if (HousenumberDifferent)
                {
                    if (Comma == true)
                    {
                        query += ",[Housenumber] = @Housenumber ";
                    } else
                    {
                        query += "[Housenumber] = @Housenumber ";
                        Comma = true;
                    }
                }
                if (AddendumDifferent)
                {
                    if (Comma == true)
                    {
                        query += ",[Addendum] = @Addendum ";
                    } else
                    {
                        query += "[Addendum] = @Addendum ";
                        Comma = true;
                    }
                }
                if (CityDifferent)
                {
                    if (Comma == true)
                    {
                        query += ",[City] = @City ";
                    } else
                    {
                        query += "[City] = @City ";
                        Comma = true;
                    }
                }
                if (PostalcodeDifferent)
                {
                    if (Comma == true)
                    {
                        query += ",[Postalcode] = @Postalcode ";
                    } else
                    {
                        query += "[Postalcode] = @Postalcode ";
                    }
                }

                query += "WHERE [Street] = @OldStreet AND [Housenumber] = @OldHousenumber AND [City] = @OldCity";
                if (!string.IsNullOrWhiteSpace(oldAddressInfo.Addendum)) query += " AND [Addendum] = @OldAddendum";
                #endregion

                using (SqlCommand command = connection.CreateCommand())
                {
                    try
                    {
                        connection.Open();
                        command.CommandText = query;

                        #region Add values of the new address if there is a difference
                        if (StreetDifferent) command.Parameters.AddWithValue("@Street", newAddressInfo.Street);
                        if (HousenumberDifferent) command.Parameters.AddWithValue("@Housenumber", newAddressInfo.Housenumber);
                        if (CityDifferent) command.Parameters.AddWithValue("@City", newAddressInfo.City);
                        if (PostalcodeDifferent) command.Parameters.AddWithValue("@Postalcode", newAddressInfo.Postalcode);
                        if (AddendumDifferent)
                        {
                            if (!string.IsNullOrWhiteSpace(newAddressInfo.Addendum))
                            {
                                command.Parameters.AddWithValue("@Addendum", newAddressInfo.Addendum);
                            } else
                            {
                                command.Parameters.AddWithValue("@Addendum", DBNull.Value);
                            }
                        }
                        #endregion
                        #region Add values of the old address in the where
                        command.Parameters.AddWithValue("@OldStreet", oldAddressInfo.Street);
                        command.Parameters.AddWithValue("@OldHousenumber", oldAddressInfo.Housenumber);
                        if (!string.IsNullOrWhiteSpace(oldAddressInfo.Addendum)) command.Parameters.AddWithValue("@OldAddendum", oldAddressInfo.Addendum);
                        command.Parameters.AddWithValue("@OldCity", oldAddressInfo.City);
                        #endregion

                        command.ExecuteNonQuery();

                    } catch (Exception ex)
                    {
                        throw new AddressRepositoryException("UpdateAddress failed - " + ex.Message);
                    } finally
                    {
                        connection.Close();
                    }
                }
            }           
        }
    }
}
