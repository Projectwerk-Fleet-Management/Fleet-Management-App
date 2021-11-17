using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ICarRepository
    {
        IReadOnlyList<Car> GetCars();
        IReadOnlyList<Car> GetCars(string? vin, string? make, string? model, string? licensePlate, string? fuelType, string? colour, string? doors, string? driverId, string? vehicleType, bool strikt = true);
        bool Exists(Car car);
        void InsertCar(Car car);
        void DeleteCar(Car car);
        void UpdateCar(Car car);
    }
}
