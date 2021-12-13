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

        public IReadOnlyList<Car> GetAllCars()
        {
            try
            {
                return _repo.GetAllCars();
            } catch (Exception e)
            {
                throw new CarmanagerException("couldn't get all cars", e);
            }
        }

        public IReadOnlyList<Car> GetCars(int? carId, string vin, string licenseplate, string make, string model, string vehicleType, List<Fuel> fueltypes, string doors, string colour)
        {
            try
            {
                return _repo.GetCars(carId, vin, licenseplate, make, model, vehicleType, fueltypes, doors, colour);

            } catch (Exception e)
            {
                throw new CarmanagerException("couldn't get cars", e);
            }
        }

        public bool Exists(int? carId, string vin, string licenseplate, string make, string model, string vehicleType, List<Fuel> fueltypes, string doors, string colour)
        {
            try
            {
                return _repo.Exists(carId, vin, licenseplate, make, model, vehicleType, fueltypes, doors, colour);
            } catch (Exception e)
            {

                throw new CarmanagerException("couldn't execute if exists", e);
            }
        }

        public void InsertCar(string vin, string licenseplate, string make, string model, string vehicleType, List<Fuel> fueltypes, string doors, string colour)
        {
            try
            {
                if (!_repo.Exists(null, vin, licenseplate, null, null, null, null, null, null))
                {
                    _repo.InsertCar(vin, licenseplate, make, model, vehicleType, fueltypes, doors, colour);
                }
                else
                {
                    throw new CarmanagerException("Car already existed");

                }
                
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
                if (_repo.Exists(car.CarId, car.Vin, car.Licenseplate, null, null, null, null, null, null))
                {
                    _repo.DeleteCar(car);
                }
                else
                {
                    throw new CarmanagerException("Car didn't exist");
                }
                
            }
            catch(Exception e)
            {
                throw new CarmanagerException("couldn't delete car", e);
            }
        }

        public void UpdateCar(Car oldCarInfo, Car newCarInfo)
        {
            try
            {
                if (_repo.Exists(oldCarInfo.CarId, oldCarInfo.Vin, oldCarInfo.Licenseplate, null, null, null, null, null, null))
                {
                    _repo.UpdateCar(oldCarInfo, newCarInfo);
                }
                else
                {
                    throw new CarmanagerException("Car does not exist");
                }
                e
            }
            catch (Exception e)
            {
                throw new CarmanagerException("can't update car", e);
            }
        }
    }
}