using BusinessLayer;
using BusinessLayer.Interfaces;
using BusinessLayer.Model;
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
                    Console.WriteLine(ex.Message);
                } finally
                {
                    connection.Close();
                }

                return addresses;
            }
        }   
        public IReadOnlyList<Address> GetAddress(int id, bool strikt = true)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Address driver)
        {
            throw new NotImplementedException();
        }

        public void DeleteDriver(Address driver)
        {
            throw new NotImplementedException();
        }

        public void InsertDriver(Address driver)
        {
            throw new NotImplementedException();
        }

        public void UpdateDriver(Address driver)
        {
            throw new NotImplementedException();
        }
    }
}
