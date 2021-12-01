using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ICarRepository
    {
        IReadOnlyList<Car> GetAllCars();
        IReadOnlyList<Car> GetCars(int? carId, string vin, string licenseplate, string make, string model, string vehicleType, List<Fuel> fueltypes, string doors, string colour);
        bool Exists(int? carId, string vin, string licenseplate, string make, string model, string vehicleType, List<Fuel> fueltypes, string doors, string colour);
        void InsertCar(string vin, string licenseplate, string make, string model, string vehicleType, List<Fuel> fueltypes, string doors, string colour);
        void DeleteCar(Car car);
        void UpdateCar(Car oldCarInfo, Car newCarInfo);
    }
}
