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
            throw new NotImplementedException();
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
