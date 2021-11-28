using BusinessLayer;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FuelcardRepository : IFuelcardRepository
    {
        private string _connectionString;

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
            string query = "SELECT * FROM [dbo].[Fuelcard] WHERE ";
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
                throw new FuelcardRepositoryException("Exists failed" + ex.Message);
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

        //Priority 1
        public void InsertFuelcard(string cardnumber, string expiryDate, List<Fuel> fueltypes, int? pincode, bool? isActive)
        {
            throw new NotImplementedException();
        }

        //Priority 1
        public void DeleteFuelcard(Fuelcard fuelcard)
        {
            throw new NotImplementedException();
        }

        //Priority 1
        public void UpdateFuelcard(Fuelcard oldFuelcard, Fuelcard newFuelcard)
        {
            throw new NotImplementedException();
        }      
    }
}
