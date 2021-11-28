using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Exceptions;
using BusinessLayer.Interfaces;

namespace BusinessLayer.Managers
{
    public class CarManager
    {
        private ICarRepository _repo;
        public CarManager(ICarRepository repo)
        {
            _repo = repo;
        }

        public IReadOnlyList<Car>GetCars()
        {
            try
            {
                return _repo.GetCars();
            }
            catch (Exception e)
            {
                throw new CarmanagerException("couldn't get all cars", e);
            }
        }

        public IReadOnlyList<Car> GetCars(string? vin, string? make, string? model, string? licensePlate, string? fuelType, string? colour, string? doors, string? driverId, string? vehicleType, bool strikt = true)
        {
            try
            {
                return _repo.GetCars(vin, make, model, licensePlate, fuelType, colour, doors, driverId, vehicleType, true);

            }
            catch (Exception e)
            {
                throw new CarmanagerException("couldn't get cars", e);
            }
        }

        public Car GetCarByVin(string vin)
        {
            try
            {
                IReadOnlyList < Car > carbyid = _repo.GetCars(vin, null, null, null, null, null, null, null, null, true);
                return carbyid.First();
            }
            catch (Exception e)
            {
                throw new CarmanagerException("couldn't get car by id", e);
            }
        }

        public bool Exists(Car car)
        {
            try
            {
                return _repo.Exists(car);
            }
            catch (Exception e)
            {

                throw new CarmanagerException("couldn't execute if exists", e);
            }
        }

        public void InsertCar(Car car)
        {
            //todo: check voor dubbels
            try
            {
                _repo.InsertCar(car);
            }
            catch (Exception e)
            {
                throw new CarmanagerException("couldn't insert car", e);
            }
        }

        public void DeleteCar(Car car)
        {
            try
            {
                _repo.DeleteCar(car);
            }
            catch(Exception e)
            {
                throw new CarmanagerException("couldn't delete car", e);
            }
        }

        public void UpdateCar(Car car)
        {
            try
            {
                //todo: check if exists
                _repo.UpdateCar(car);
            }
            catch (Exception e)
            {
                throw new CarmanagerException("can't update car", e);
            }
        }
    }
}