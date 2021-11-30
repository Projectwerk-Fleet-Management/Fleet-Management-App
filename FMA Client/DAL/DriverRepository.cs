using BusinessLayer;
using BusinessLayer.Interfaces;
using System;
using System.Collections.Generic;
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

        public void DeleteDriver(Driver driver)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int? driverId, string firstName, string lastName, string dateOfBirth, int? nationalIdentificationNumber, string licenses)
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

        public IReadOnlyList<Driver> GetAllDrivers()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Driver> GetDrivers(int? driverId, string firstName, string lastName, string dateOfBirth, int? nationalIdentificationNumber, string licenses)
        {
            throw new NotImplementedException();
        }

        public void InsertDriver(string firstName, string lastName, string dateOfBirth, int nationalIdentificationNumber, string licenses)
        {
            throw new NotImplementedException();
        }

        public void UpdateDriver(Driver oldDriverInfo, Driver newDriverInfo)
        {
            throw new NotImplementedException();
        }



    }
}
