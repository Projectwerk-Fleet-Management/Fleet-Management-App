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

        public IReadOnlyList<Fuelcard> GetFuelcard(int? fuelcardId, string cardnumber, DateTime? expiryDate, List<Fuel> fueltypes, bool? isActive)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int? fuelcardId, string cardnumber, DateTime? expiryDate, List<Fuel> fueltypes, bool? isActive)
        {
            throw new NotImplementedException();
        }

        public void InsertFuelcard(string cardnumber, DateTime expiryDate, List<Fuel> fueltypes, int? pincode, bool? isActive)
        {
            throw new NotImplementedException();
        }

        public void DeleteFuelcard(Fuelcard fuelcard)
        {
            throw new NotImplementedException();
        }

        public void UpdateFuelcard(Fuelcard oldFuelcard, Fuelcard newFuelcard)
        {
            throw new NotImplementedException();
        }      
    }
}
