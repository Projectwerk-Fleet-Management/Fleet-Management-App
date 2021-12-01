using BusinessLayer;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
using BusinessLayer.Validators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class FuelcardRepository : IFuelcardRepository
    {
        private string _connectionString;
        private FuelcardNumberValidator fuelcardValidator = new();

        public FuelcardRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            return connection;
        }

        //Done
        public IReadOnlyList<Fuelcard> GetAllFuelcards()
        {
            List<Fuelcard> fuelcards = new();
            SqlConnection connection = getConnection();
            string query = "SELECT * FROM dbo.Fuelcard";

            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    SqlDataReader datareader = command.ExecuteReader();
                    while (datareader.Read())
                    {
                        int fuelcardId = (int)datareader["FuelcardId"];
                        string cardnumber = (string)datareader["Cardnumber"];
                        DateTime expiryDate = (DateTime)datareader["ExpiryDate"];
                        bool isActive = (bool)datareader["IsActive"];
                        List<Fuel> fueltypesList = new();

                        #region Null checking
                        if (DBNull.Value != datareader["Fueltypes"])
                        {
                            #region Db fueltype to program
                            //In the database the fueltypes are put in as a string with a "," seperating each type.
                            //This basically turns it into a list for us to use
                            //We're also making the fueltypes all upper case in the array to eliminate the case sensitivity
                            string fueltypesString = (string)datareader["Fueltypes"];
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
                        #endregion

                        Fuelcard fuelcard = new(fuelcardId, cardnumber, expiryDate, /*pincode,*/ fueltypesList, isActive);

                        fuelcards.Add(fuelcard);
                    }
                    datareader.Close();

                } catch (Exception ex)
                {
                    throw new FuelcardRepositoryException("GetAllFuelcards failed" + ex.Message);
                } finally
                {
                    connection.Close();
                }

                return fuelcards;
            }
        }

        //Done
        public IReadOnlyList<Fuelcard> GetFuelcard(int? fuelcardId, string cardnumber, string expiryDate, List<Fuel> fueltypes, bool? isActive)
        {
            bool AND = false;
            SqlConnection connection = getConnection();
            string query = "SELECT * FROM [dbo].[Fuelcard] ";
            #region Add to the query for search function
            if (fuelcardId != null)
            {
                query += "WHERE FuelcardId = @Id ";
                AND = true;
            }
            if (!string.IsNullOrWhiteSpace(cardnumber))
            {
                if (AND == true)
                {
                    query += "AND Cardnumber = @Cardnumber ";
                } else
                {
                    query += "WHERE Cardnumber = @Cardnumber ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(expiryDate))
            {
                if (AND == true)
                {
                    query += "AND ExpiryDate = @ExpiryDate ";
                } else
                {
                    query += "WHERE ExpiryDate = @ExpiryDate ";
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
            if (isActive != null)
            {
                if (AND == true)
                {
                    query += "AND IsActive = @IsActive ";
                } else
                {
                    query += "WHERE IsActive = @IsActive ";
                    AND = true;
                }
            }
            #endregion

            using SqlCommand command = new(query, connection);
            try
            {
                connection.Open();
                #region Add values to the search terms
                if (fuelcardId != null)
                {
                    command.Parameters.AddWithValue("@Id", fuelcardId);
                }
                if (!string.IsNullOrWhiteSpace(cardnumber))
                {
                    command.Parameters.AddWithValue("@Cardnumber", cardnumber);
                }
                if (!string.IsNullOrWhiteSpace(expiryDate))
                {
                    command.Parameters.AddWithValue("@ExpiryDate", expiryDate);
                }
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
                if (isActive != null)
                {
                    //Here we need to transform the isActive to a bit seeing as that's how it's stored in the Db
                    int isActiveInBit;
                    if (isActive == true) isActiveInBit = 1; else isActiveInBit = 0;

                    command.Parameters.AddWithValue("@IsActive", isActiveInBit);
                }
                #endregion

                List<Fuelcard> fuelcards = new();
                SqlDataReader datareader = command.ExecuteReader();
                while (datareader.Read())
                {
                    int fuelcardIdDb = (int)datareader["FuelcardId"];
                    string cardnumberDb = (string)datareader["Cardnumber"];
                    DateTime expiryDateDb = (DateTime)datareader["ExpiryDate"];
                    bool isActiveDb = (bool)datareader["IsActive"];

                    List<Fuel> fueltypesList = new();
                    #region Null checking
                    if (DBNull.Value != datareader["Fueltypes"])
                    {
                        #region Transform string to fueltype list     
                        string fueltypesDbString = (string)datareader["Fueltypes"];
                        fueltypesDbString = fueltypesDbString.Replace(" ", "");
                        string[] fueltypesArray = fueltypesDbString.ToUpper().Split(",");

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
                    #endregion

                    Fuelcard fc = new(fuelcardIdDb, cardnumberDb, expiryDateDb, fueltypesList, isActiveDb);
                    fuelcards.Add(fc);
                }

                return fuelcards;
            } catch (Exception ex)
            {
                throw new FuelcardRepositoryException("GetFuelcard failed" + ex.Message);
            } finally
            {
                connection.Close();
            }
        }

        //Done
        public bool Exists(int? fuelcardId, string cardnumber, string expiryDate, List<Fuel> fueltypes, bool? isActive)
        {
            bool AND = false;
            SqlConnection connection = getConnection();
            string query = "SELECT count(*) FROM [dbo].[Fuelcard] WHERE ";
            #region Add to the query for search function
            if (fuelcardId != null)
            {
                query += "FuelcardId = @Id ";
                AND = true;
            }
            if (!string.IsNullOrWhiteSpace(cardnumber))
            {
                if (AND == true)
                {
                    query += "AND Cardnumber = @Cardnumber ";
                } else
                {
                    query += "Cardnumber = @Cardnumber ";
                    AND = true;
                }
            }
            if (!string.IsNullOrWhiteSpace(expiryDate))
            {
                if (AND == true)
                {
                    query += "AND ExpiryDate = @ExpiryDate ";
                } else
                {
                    query += "ExpiryDate = @ExpiryDate ";
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
                    query += "Fueltypes = @Fueltypes ";
                    AND = true;
                }
            }
            if (isActive != null)
            {
                if (AND == true)
                {
                    query += "AND IsActive = @IsActive ";
                } else
                {
                    query += "IsActive = @IsActive ";
                    AND = true;
                }
            }
            #endregion

            using SqlCommand command = new(query, connection);
            try
            {
                connection.Open();
                #region Add values to the search terms
                if (fuelcardId != null)
                {
                    command.Parameters.AddWithValue("@Id", fuelcardId);
                }
                if (!string.IsNullOrWhiteSpace(cardnumber))
                {
                    command.Parameters.AddWithValue("@Cardnumber", cardnumber);
                }
                if (!string.IsNullOrWhiteSpace(expiryDate))
                {
                    command.Parameters.AddWithValue("@ExpiryDate", expiryDate);
                }
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
                if (isActive != null)
                {
                    //Here we need to transform the isActive to a bit seeing as that's how it's stored in the Db
                    int isActiveInBit;
                    if (isActive == true) isActiveInBit = 1; else isActiveInBit = 0;

                    command.Parameters.AddWithValue("@IsActive", isActiveInBit);
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
                throw new FuelcardRepositoryException("Exists failed" + ex.Message);
            } finally
            {
                connection.Close();
            }
        }

        //Done
        public void InsertFuelcard(string cardnumber, string expiryDate, List<Fuel> fueltypes, int? pincode, bool? isActive)
        {
            #region Check if given variables aren't empty or invalid
            if (string.IsNullOrWhiteSpace(cardnumber))
            {
                throw new FuelcardRepositoryException("InsertFuelcard - Cardnumber cannot be empty");
            }
            if (fuelcardValidator.isValid(cardnumber));
            {
                throw new FuelcardRepositoryException("InsertFuelcard - Cardnumber is not valid");
            }
            if (string.IsNullOrWhiteSpace(expiryDate))
            {
                throw new FuelcardRepositoryException("InsertFuelcard - Expiry date cannot be empty");
            }
            if (isActive == null)
            {
                throw new FuelcardRepositoryException("InsertFuelcard - IsActive cannot be empty");
            }
            if (pincode < 999 || pincode > 1000000)
            {
                throw new FuelcardRepositoryException("InsertFuelcard - Pincode is not valid, it needs to be between 4 and 6 numbers");
            }
            #endregion

            #region Check if given variables aren't already in the database
            if (Exists(null, cardnumber, null, null, null))
            {
                throw new FuelcardRepositoryException("InsertFuelcard - Fuelcard already exists in database");
            }
            #endregion

            SqlConnection connection = getConnection();
            string query = "INSERT INTO dbo.Fuelcard ([Cardnumber], [ExpiryDate], [Pincode], [Fueltypes] ,[IsActive])" +
                " VALUES (@Cardnumber, @ExpiryDate, @Pincode, @Fueltypes, @IsActive)";

            using (SqlCommand command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@Cardnumber", cardnumber);
                    command.Parameters.AddWithValue("@ExpiryDate", expiryDate);
                    command.Parameters.AddWithValue("@IsActive", isActive);

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
                    if (pincode != null)
                    {
                        command.Parameters.AddWithValue("@Pincode", pincode);
                    } else
                    {
                        command.Parameters.AddWithValue("@Pincode", DBNull.Value);
                    }
                    #endregion

                    command.ExecuteNonQuery();

                } catch (Exception ex)
                {
                    throw new FuelcardRepositoryException("InsertFuelcard failed - " + ex.Message);
                } finally
                {
                    connection.Close();
                }
            }
        }

        //Done
        public void DeleteFuelcard(Fuelcard fuelcard)
        {
            //Only need to delete based on fuelcardId actually but we can use the cardnumber as a sort of protection
            SqlConnection connection = getConnection();
            string query = "Delete FROM [dbo].[Fuelcard] WHERE FuelcardId = @Id AND Cardnumber = @Cardnumber";

            using SqlCommand command = new(query, connection);
            try
            {
                connection.Open();
                #region Add values to the search terms
                command.Parameters.AddWithValue("@Id", fuelcard.FuelcardId);
                command.Parameters.AddWithValue("@Cardnumber", fuelcard.Cardnumber);
                #endregion

                command.ExecuteNonQuery();

            } catch (Exception ex)
            {
                throw new AddressRepositoryException("DeleteFuelcard failed - " + ex.Message);
            } finally
            {
                connection.Close();
            }
        }

        //Done
        public void UpdateFuelcard(Fuelcard oldFuelcard, Fuelcard newFuelcard)
        {
            bool Comma = false;

            #region Check if there is a difference with the old address (The old address is already retrieved from the database
            bool CardnumberDifferent = oldFuelcard.Cardnumber != newFuelcard.Cardnumber;
            bool ExpiryDateDifferent = oldFuelcard.ExpiryDate != newFuelcard.ExpiryDate;
            bool PincodeDifferent = oldFuelcard.Pincode != newFuelcard.Pincode;
            bool FueltypesDifferent = oldFuelcard.FueltypeList != newFuelcard.FueltypeList;
            bool IsActiveDifferent = oldFuelcard.isActive != newFuelcard.isActive;
            #endregion

            if (CardnumberDifferent || ExpiryDateDifferent || PincodeDifferent || FueltypesDifferent || IsActiveDifferent)
            {
                SqlConnection connection = getConnection();
                string query = "UPDATE dbo.Fuelcard SET ";

                #region For each thing different add it into the query and at the end add the where
                if (CardnumberDifferent)
                {
                    query += "[Cardnumber] = @Cardnumber ";
                    Comma = true;
                }
                if (ExpiryDateDifferent)
                {
                    if (Comma == true)
                    {
                        query += ",[ExpiryDate] = @ExpiryDate ";
                    } else
                    {
                        query += "[ExpiryDate] = @ExpiryDate ";
                        Comma = true;
                    }
                }
                if (PincodeDifferent)
                {
                    if (Comma == true)
                    {
                        query += ",[Pincode] = @Pincode ";
                    } else
                    {
                        query += "[Pincode] = @Pincode ";
                        Comma = true;
                    }
                }
                if (FueltypesDifferent)
                {
                    if (Comma == true)
                    {
                        query += ",[Fueltypes] = @Fueltypes ";
                    } else
                    {
                        query += "[Fueltypes] = @Fueltypes ";
                        Comma = true;
                    }
                }
                if (IsActiveDifferent)
                {
                    if (Comma == true)
                    {
                        query += ",[IsActive] = @IsActive ";
                    } else
                    {
                        query += "[IsActive] = @IsActive ";
                    }
                }

                query += "WHERE [FuelcardId] = @OldFuelcardId AND [Cardnumber] = @OldCardnumber";
                #endregion

                using (SqlCommand command = connection.CreateCommand())
                {
                    try
                    {
                        connection.Open();
                        command.CommandText = query;

                        #region Add values of the new address if there is a difference
                        if (CardnumberDifferent) command.Parameters.AddWithValue("@Cardnumber", newFuelcard.Cardnumber);
                        if (ExpiryDateDifferent) command.Parameters.AddWithValue("@ExpiryDate", newFuelcard.ExpiryDate.ToString("yyyy/MM/dd"));
                        if (PincodeDifferent) command.Parameters.AddWithValue("@Pincode", newFuelcard.Pincode);
                        if (FueltypesDifferent)
                        {
                            string fueltypesString = "";
                            bool first = true;

                            foreach (var f in newFuelcard.FueltypeList)
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
                        if (IsActiveDifferent) command.Parameters.AddWithValue("@Postalcode", newFuelcard.isActive);

                        #endregion
                        #region Add values of the old address in the where
                        command.Parameters.AddWithValue("@OldFuelcardId", oldFuelcard.FuelcardId);
                        command.Parameters.AddWithValue("@OldCardnumber", oldFuelcard.Cardnumber);
                        #endregion

                        command.ExecuteNonQuery();


                    } catch (Exception ex)
                    {
                        throw new AddressRepositoryException("UpdateFuelcard failed - " + ex.Message);
                    } finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
