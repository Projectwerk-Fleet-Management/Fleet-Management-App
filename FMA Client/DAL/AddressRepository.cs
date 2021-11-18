using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Model;
using BusinessLayer.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
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

        //Not completely done, need to work with the new constructors we use (Nullables) Priority nr 1
        public IReadOnlyList<Address> GetAllAddresses()
        {
            List<Address> addresses = new();
            SqlConnection connection = getConnection();
            string query = "SELECT * FROM dbo.Address";

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
                        int driverId;

                        #region Null checking
                        if (DBNull.Value != datareader["Addendum"])
                        {
                            addendum = (string)datareader["Addendum"];
                        }
                        if (DBNull.Value != datareader["DriverId"])
                        {
                            driverId = (int)datareader["DriverId"];
                        }
                        #endregion

                        Address address = new(addressId, street, housenumber, addendum, city, postalcode);

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

        //Priority nr 5
        public IReadOnlyList<Address> GetAddress(int id, bool strikt = true)
        {
            throw new NotImplementedException();
        }

        //Done? Maybe add ways to check if it exists based on street + number?
        public bool Exists(int id)
        {
            SqlConnection connection = getConnection();
            string sql = "SELECT count(*) FROM [dbo].[Address] WHERE AddressId = @Id";
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@Id", id);
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

        //Priority nr 3
        public void DeleteDriver(Address address)
        {
            throw new NotImplementedException();
        }

        //On going -> Priority nr 2
        public void InsertDriver(Address address)
        {
            /*
            SqlConnection connection = getConnection();
            string query = "INSERT INTO dbo.Address ([Street],[Housenumber],[Addendum],[Postalcode],[City]) VALUES (?, ?, ?, ?, ?)";

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
                        int driverId;

                        #region Null checking
                        if (DBNull.Value != datareader["Addendum"])
                        {
                            addendum = (string)datareader["Addendum"];
                        }
                        if (DBNull.Value != datareader["DriverId"])
                        {
                            driverId = (int)datareader["DriverId"];
                        }
                        #endregion

                        Address address = new(addressId, street, housenumber, addendum, city, postalcode);

                        addresses.Add(address);
                    }
                    datareader.Close();

                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                } finally
                {
                    connection.Close();
                }

                return addresses;
            }
            */
        }

        //Priority nr 4
        public void UpdateDriver(Address address)
        {
            throw new NotImplementedException();
        }
    }
}
