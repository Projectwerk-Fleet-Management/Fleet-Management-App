using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DriverRepository : IDriverRepository
    {
        private string _connectionString;

        public DriverRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            return connection;
        }

        public IReadOnlyList<Driver> GetAllDrivers()
        {
            List<Driver> drivers = new();
            SqlConnection connection = getConnection();
            string query = "SELECT * FROM dbo.Driver";

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
                        string licensesDb = "";
                        int addressId = 0;
                        int fuelcardId = 0;
                        int carId = 0;

                        #region Null checking
                        if (DBNull.Value != datareader["Licenses"])
                        {
                            licensesDb = (string)datareader["Licenses"];
                        }
                        if (DBNull.Value != datareader["AddressId"])
                        {
                            addressId = (int)datareader["AddressId"];
                        }
                        if (DBNull.Value != datareader["FuelcardId"])
                        {
                            fuelcardId = (int)datareader["FuelcardId"];
                        }
                        if (DBNull.Value != datareader["CarId"])
                        {
                            carId = (int)datareader["CarId"];
                        }
                        #endregion

                        List<LicenseType> licenses = new();
                        #region Check if licenseDb is valid in the enum and add it to the licenses -> Also do a for loop for each license in licensesDb
                        if (!string.IsNullOrWhiteSpace(licensesDb))
                        {

                            licensesDb = licensesDb.Replace(" ", "");
                            string[] licensesList = licensesDb.ToUpper().Split(",");
                            foreach (string s in licensesList)
                            {
                                if (string.IsNullOrWhiteSpace(s)) { continue; }
                                if (s == LicenseType.A.ToString())
                                {
                                    licenses.Add(LicenseType.A);
                                } else if (s == LicenseType.A1.ToString())
                                {
                                    licenses.Add(LicenseType.A1);
                                } else if (s == LicenseType.A2.ToString())
                                {
                                    licenses.Add(LicenseType.A2);
                                } else if (s == LicenseType.AM.ToString())
                                {
                                    licenses.Add(LicenseType.AM);
                                } else if (s == LicenseType.B.ToString())
                                {
                                    licenses.Add(LicenseType.B);
                                } else if (s == LicenseType.BE.ToString())
                                {
                                    licenses.Add(LicenseType.BE);
                                } else if (s == LicenseType.C.ToString())
                                {
                                    licenses.Add(LicenseType.C);
                                } else if (s == LicenseType.C1.ToString())
                                {
                                    licenses.Add(LicenseType.C1);
                                } else if (s == LicenseType.C1E.ToString())
                                {
                                    licenses.Add(LicenseType.C1E);
                                } else if (s == LicenseType.CE.ToString())
                                {
                                    licenses.Add(LicenseType.CE);
                                } else if (s == LicenseType.D.ToString())
                                {
                                    licenses.Add(LicenseType.D);
                                } else if (s == LicenseType.D1.ToString())
                                {
                                    licenses.Add(LicenseType.D1);
                                } else if (s == LicenseType.D1E.ToString())
                                {
                                    licenses.Add(LicenseType.D1E);
                                } else if (s == LicenseType.DE.ToString())
                                {
                                    licenses.Add(LicenseType.DE);
                                } else if (s == LicenseType.G.ToString())
                                {
                                    licenses.Add(LicenseType.G);
                                }
                            }
                        }
                        #endregion

                        //Driver driver = new(driverId, lastName, firstName, dateOfBirth, nationalIdentificationNumber, licenses, addressId, carId, fuelcardId);
                        //drivers.Add(driver);
                    }
                    datareader.Close();

                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                } finally
                {
                    connection.Close();
                }

                return drivers;
            }
        }

        public IReadOnlyList<Driver> GetDrivers(string? id, string? firstName, string? lastName, DateTime? dateOfBirth, string? nationalIdentificationNumber, string? vin, string? fuelcardNumber, string? license, bool strikt = true)
        {
            List<Driver> drivers = new();
            SqlConnection connection = getConnection();
            string query = "SELECT * FROM dbo.Driver";
            bool WHERE = false;
            bool AND = false;

            if (!string.IsNullOrWhiteSpace(id))
            {
                WHERE = true;
                query += " WHERE";
                AND = true;

                if (strikt)
                {
                    query += " id=@id";
                } else
                {
                    query += " UPPER(id)=UPPER(@id)";
                }

            }
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                if (!WHERE) query += " WHERE"; WHERE = true;
                if (AND) query += " AND"; else AND = true;

                if (strikt)
                {
                    query += " firstName=@firstName";
                } else
                {
                    query += " UPPER(firstname)=UPPER(@firstName)";
                }
            }
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                if (!WHERE) query += " WHERE"; WHERE = true;
                if (AND) query += " AND"; else AND = true;

                if (strikt)
                {
                    query += " lastName=@lastName";
                } else
                {
                    query += " UPPER(lastName)=UPPER(@lastName)";
                }
            }
            if (!string.IsNullOrWhiteSpace(nationalIdentificationNumber))
            {
                if (!WHERE) query += " WHERE"; WHERE = true;
                if (AND) query += " AND"; else AND = true;

                if (strikt)
                {
                    query += " nationalIdentificationNumber=@nationalIdentificationNumber";
                } else
                {
                    query += " UPPER(nationalIdentificationNumber)=UPPER(@nationalIdentificationNumber)";
                }
            }
            if (!string.IsNullOrWhiteSpace(vin))
            {
                if (!WHERE) query += " WHERE"; WHERE = true;
                if (AND) query += " AND"; else AND = true;

                if (strikt)
                {
                    query += " assignedCar=@vin";
                } else
                {
                    query += " UPPER(assignedCar)=UPPER(@vin)";
                }
            }
            if (!string.IsNullOrWhiteSpace(fuelcardNumber))
            {
                if (!WHERE) query += " WHERE"; WHERE = true;
                if (AND) query += " AND"; else AND = true;

                if (strikt)
                {
                    query += " assignedFuelcard=@fuelcardNumber";
                } else
                {
                    query += " UPPER(assignedFuelcard)=UPPER(@fuelcardNumber)";
                }
            }
            if (!string.IsNullOrWhiteSpace(license))
            {
                if (!WHERE) query += " WHERE"; WHERE = true;
                if (AND) query += " AND "; else AND = true;

                if (strikt)
                {
                    query += " licenses LIKE '%@license%'";
                } else
                {
                    query += " UPPER(licenses) LIKE UPPER('%@license%')";
                }
            }
            if (dateOfBirth != null)
            {
                if (!WHERE) query += " WHERE"; WHERE = true;
                if (AND) query += " AND"; else AND = true;

                if (strikt)
                {
                    query += " dateOfBirth=@dateOfBirth";
                } else
                {
                    query += " UPPER(dateOfBirth)=UPPER(@dateOfBirth)";
                }
            }

            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        command.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar));
                        command.Parameters[@id].Value = id;
                    }
                    if (!string.IsNullOrWhiteSpace(firstName))
                    {
                        command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar));
                        command.Parameters[@firstName].Value = firstName;
                    }
                    if (!string.IsNullOrWhiteSpace(lastName))
                    {
                        command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar));
                        command.Parameters[@lastName].Value = lastName;
                    }
                    if (!string.IsNullOrWhiteSpace(nationalIdentificationNumber))
                    {
                        command.Parameters.Add(new SqlParameter("@nationalIdentificationNumber", SqlDbType.NVarChar));
                        command.Parameters[@nationalIdentificationNumber].Value = nationalIdentificationNumber;
                    }
                    if (!string.IsNullOrWhiteSpace(vin))
                    {
                        command.Parameters.Add(new SqlParameter("@vin", SqlDbType.NVarChar));
                        command.Parameters[@vin].Value = vin;
                    }
                    if (!string.IsNullOrWhiteSpace(fuelcardNumber))
                    {
                        command.Parameters.Add(new SqlParameter("@fuelcardNumber", SqlDbType.NVarChar));
                        command.Parameters[@fuelcardNumber].Value = fuelcardNumber;
                    }

                    if (!string.IsNullOrWhiteSpace(license))
                    {
                        command.Parameters.Add(new SqlParameter("@licenses", SqlDbType.NVarChar));
                        //command.Parameters[@licenses].Value = license;
                    }
                    if (dateOfBirth != null)
                    {
                        command.Parameters.Add(new SqlParameter("@dateOfBirth", SqlDbType.Timestamp));
                        //command.Parameters[@dateOfBirth].Value = dateOfBirth;
                    }
                    command.CommandText = query;

                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        //TODO: add the read object to the list
                    }
                } catch (Exception)
                {
                    throw;
                }
            }

            return drivers;
        }
        
        /*
        public Driver GetDriverById(int id)
        {
            Driver driver;
            SqlConnection connection = getConnection();
            string query = "SELECT * FROM dbo.Driver WHERE id = @id";

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
                        string licensesDb = "";
                        int addressId = 0;
                        int fuelcardId = 0;
                        int carId = 0;

                        #region Null checking
                        if (DBNull.Value != datareader["Licenses"])
                        {
                            licensesDb = (string)datareader["Licenses"];
                        }
                        if (DBNull.Value != datareader["AddressId"])
                        {
                            addressId = (int)datareader["AddressId"];
                        }
                        if (DBNull.Value != datareader["FuelcardId"])
                        {
                            fuelcardId = (int)datareader["FuelcardId"];
                        }
                        if (DBNull.Value != datareader["CarId"])
                        {
                            carId = (int)datareader["CarId"];
                        }
                        #endregion

                        List<LicenseType> licenses = new();
                        #region Check if licenseDb is valid in the enum and add it to the licenses -> Also do a for loop for each license in licensesDb
                        if (!string.IsNullOrWhiteSpace(licensesDb))
                        {

                            licensesDb = licensesDb.Replace(" ", "");
                            string[] licensesList = licensesDb.ToUpper().Split(",");
                            foreach (string s in licensesList)
                            {
                                if (string.IsNullOrWhiteSpace(s)) { continue; }
                                if (s == LicenseType.A.ToString())
                                {
                                    licenses.Add(LicenseType.A);
                                } else if (s == LicenseType.A1.ToString())
                                {
                                    licenses.Add(LicenseType.A1);
                                } else if (s == LicenseType.A2.ToString())
                                {
                                    licenses.Add(LicenseType.A2);
                                } else if (s == LicenseType.AM.ToString())
                                {
                                    licenses.Add(LicenseType.AM);
                                } else if (s == LicenseType.B.ToString())
                                {
                                    licenses.Add(LicenseType.B);
                                } else if (s == LicenseType.BE.ToString())
                                {
                                    licenses.Add(LicenseType.BE);
                                } else if (s == LicenseType.C.ToString())
                                {
                                    licenses.Add(LicenseType.C);
                                } else if (s == LicenseType.C1.ToString())
                                {
                                    licenses.Add(LicenseType.C1);
                                } else if (s == LicenseType.C1E.ToString())
                                {
                                    licenses.Add(LicenseType.C1E);
                                } else if (s == LicenseType.CE.ToString())
                                {
                                    licenses.Add(LicenseType.CE);
                                } else if (s == LicenseType.D.ToString())
                                {
                                    licenses.Add(LicenseType.D);
                                } else if (s == LicenseType.D1.ToString())
                                {
                                    licenses.Add(LicenseType.D1);
                                } else if (s == LicenseType.D1E.ToString())
                                {
                                    licenses.Add(LicenseType.D1E);
                                } else if (s == LicenseType.DE.ToString())
                                {
                                    licenses.Add(LicenseType.DE);
                                } else if (s == LicenseType.G.ToString())
                                {
                                    licenses.Add(LicenseType.G);
                                }
                            }
                        }
                        #endregion

                        driver = new(driverId, lastName, firstName, dateOfBirth, nationalIdentificationNumber, licenses, addressId, carId, fuelcardId);
                    }
                    datareader.Close();

                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                } finally
                {
                    connection.Close();
                    return driver;
                }              
            }
        }
        */
        public void DeleteDriver(Driver driver)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Driver driver)
        {
            throw new NotImplementedException();
        }

        public void InsertDriver(Driver driver)
        {
            throw new NotImplementedException();
        }

        public void UpdateDriver(Driver driver)
        {
            throw new NotImplementedException();
        }
    }
}
