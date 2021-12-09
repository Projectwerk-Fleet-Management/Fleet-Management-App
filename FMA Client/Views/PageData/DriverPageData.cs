using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using BusinessLayer;

namespace Views.PageData
{
    public class DriverPageData
    {
        private Dictionary<string, Driver> loadedData = new Dictionary<string, Driver>();

        public void setList(IReadOnlyList<Driver> driverlistDrivers)
        {
            loadedData.Clear();
            foreach (var Driver in driverlistDrivers)
            {
                loadedData.Add($"{Driver.DriverId}: {Driver.FirstName} {Driver.LastName}", Driver);
            }
        }

        public List<string> getFill()
        {
            List<string> togive = new List<string>();
            foreach (var keypairvalue in loadedData)
            {
                togive.Add(keypairvalue.Key);
            }

            return togive;
        }

        public Driver getDriverDetails(string x)
        {
            return loadedData[x];
        }
    }
}