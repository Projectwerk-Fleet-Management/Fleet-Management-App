using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Managers
{
    public class CarManager
    {
        private ICarRepository _repo;
        public CarManager(ICarRepository repo)
        {
            _repo = repo;
        }

        public IReadOnlyList<Car> GetCars()
        {
            try
            {
                return _repo.GetAllCars();
            } catch (Exception e)
            {
                throw new CarmanagerException("couldn't get all cars", e);
            }
        }

        public IReadOnlyList<Car> GetCars(int? carId, string vin, string licenseplate, string make, string model, string vehicleType, string fueltypes, string doors, string colour)
        {
            try
            {
                return _repo.GetCars(carId, vin, licenseplate, make, model, vehicleType, fueltypes, doors, colour);

            } catch (Exception e)
            {
                throw new CarmanagerException("couldn't get cars", e);
            }
        }

        public bool Exists(int? carId, string vin, string licenseplate, string make, string model, string vehicleType, string fueltypes, string doors, string colour)
        {
            try
            {
                return _repo.Exists(carId, vin, licenseplate, make, model, vehicleType, fueltypes, doors, colour);
            } catch (Exception e)
            {

                throw new CarmanagerException("couldn't execute if exists", e);
            }
        }

        public void InsertCar(string vin, string licenseplate, string make, string model, string vehicleType, string fueltypes, string doors, string colour)
        {
            //todo: check voor dubbels
            try
            {
                _repo.InsertCar(vin, licenseplate, make, model, vehicleType, fueltypes, doors, colour);
            } catch (Exception e)
            {
                throw new CarmanagerException("couldn't insert car", e);
            }
        }

        public void DeleteCar(Car car)
        {
            try
            {
                _repo.DeleteCar(car);
            } catch (Exception e)
            {
                throw new CarmanagerException("couldn't delete car", e);
            }
        }

        public void UpdateCar(Car oldCarInfo, Car newCarInfo)
        {
            try
            {
                //todo: check if exists
                _repo.UpdateCar(oldCarInfo, newCarInfo);
            } catch (Exception e)
            {
                throw new CarmanagerException("can't update car", e);
            }
        }
    }
}