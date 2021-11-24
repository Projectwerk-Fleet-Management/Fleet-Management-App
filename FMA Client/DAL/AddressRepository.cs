using BusinessLayer;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
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

                        #region Null checking
                        if (DBNull.Value != datareader["Addendum"])
                        {
                            addendum = (string)datareader["Addendum"];
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

        //Done
        public IReadOnlyList<Address> GetAddress(int? id, string street, string housenumber, string addendum, string city, int? postalcode)
        {
            List<Address> addresses = new();
            bool AND = false;
            SqlConnection connection = getConnection();
            string query = "SELECT * FROM [dbo].[Address] WHERE ";
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
                    command.Parameters.AddWithValue("@Housnumber", housenumber);
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

                    #region Null checking
                    if (DBNull.Value != datareader["Addendum"])
                    {
                        addendumDb = (string)datareader["Addendum"];
                    }
                    #endregion
                    #endregion

                    Address address = new(addressIdDb, streetDb, housenumberDb, addendumDb, cityDb, postalcodeDb);

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
        public void DeleteAddress(Address address)
        {
            #region Check if at least street, housenumber and city has been given, is this actually needed?
            if (string.IsNullOrWhiteSpace(address.Street)) throw new AddressRepositoryException("DeleteAddress - Street cannot be empty");
            if (string.IsNullOrWhiteSpace(address.Housenumber)) throw new AddressRepositoryException("DeleteAddress - Housenumber cannot be empty");
            if (string.IsNullOrWhiteSpace(address.City)) throw new AddressRepositoryException("DeleteAddress - City cannot be empty");
            #endregion

            SqlConnection connection = getConnection();
            string query = "Delete FROM [dbo].[Address] WHERE AddressId = @Id AND Street = @Street AND" +
                " Housenumber = @Housenumber AND City = @City AND Postalcode = @Postalcode";

            if (!string.IsNullOrWhiteSpace(address.Addendum)) query +=" AND Addendum = @Addendum";

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
        public void UpdateAddress(Address oldAddressInfo, Address newAddressInfo)
        {
            #region Check if given variables aren't empty or invalid of the new address (Old one should already be correct)
            if (string.IsNullOrWhiteSpace(newAddressInfo.Street))
            {
                throw new AddressRepositoryException("UpdateDriver - Street cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(newAddressInfo.Housenumber))
            {
                throw new AddressRepositoryException("UpdateDriver - Housenumber cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(newAddressInfo.City))
            {
                throw new AddressRepositoryException("UpdateDriver - City cannot be empty");
            }
            if (newAddressInfo.Postalcode < 1000 || newAddressInfo.Postalcode > 9992)
            {
                throw new AddressRepositoryException("UpdateDriver - Postalcode is not a valid number");
            }
            #endregion

            bool Comma = false;

            #region Check if there is a difference with the old address
            bool StreetDifferent = oldAddressInfo.Street.ToUpper() != newAddressInfo.Street.ToUpper();
            bool HousenumberDifferent = oldAddressInfo.Housenumber.ToUpper() != newAddressInfo.Housenumber.ToUpper();
            bool AddendumDifferent = oldAddressInfo.Addendum != newAddressInfo.Addendum;
            bool CityDifferent = oldAddressInfo.City.ToUpper() != newAddressInfo.City.ToUpper();
            bool PostalcodeDifferent = oldAddressInfo.Postalcode != newAddressInfo.Postalcode;
            #endregion

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
