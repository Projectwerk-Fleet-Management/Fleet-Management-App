using BusinessLayer;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
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
            string query = "SELECT * FROM dbo.Car";

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
                        #region Null checking
                        if (DBNull.Value != datareader["Doors"])
                        {
                            doors = (string)datareader["Doors"];
                        }
                        if (DBNull.Value != datareader["Colour"])
                        {
                            colour = (string)datareader["Colour"];
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

                        Car car = new(carId, make, model, vin, licenseplate, vehicleType, fueltypesList, colour, doors);

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
            string query = "SELECT * FROM dbo.Car ";

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
                        #region Null checking
                        if (DBNull.Value != datareader["Doors"])
                        {
                            doorsDb = (string)datareader["Doors"];
                        }
                        if (DBNull.Value != datareader["Colour"])
                        {
                            colourDb = (string)datareader["Colour"];
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

                        Car car = new(carIdDb, makeDb, modelDb, vinDb, licenseplateDb, vehicleTypeDb, fueltypesList, colourDb, doorsDb);

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
