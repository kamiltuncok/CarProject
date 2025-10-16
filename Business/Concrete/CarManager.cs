using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Performance;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Business.Concrete
{
    public class CarManager : ICarService
    {
        ICarDal _carDal;

        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }

        //[SecuredOperation("car.add,admin")]
        [ValidationAspect(typeof(CarValidator))]
        [CacheRemoveAspect("ICarService.Get")]
        public IResult Add(Car car)
        {
            car.IsRented = false;
            _carDal.Add(car);
            return new SuccessResult(Messages.CarAdded);
        }

        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Car car)
        {
            Add(car);
            if (car.DailyPrice < 10)
            {
                throw new Exception("");
            }
            Add(car);

            return null;
        }

        public IDataResult<List<CarDetailDto>> GetCarDetails()
        {

            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetails(), Messages.carDetailsListed);
        }

        public IResult Delete(Car car)
        {
            _carDal.Delete(car);
            return new SuccessResult(Messages.CarDeleted);
        }


        [CacheAspect]
        [PerformanceAspect(5)]
        public IDataResult<List<Car>> GetAll()
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(), Messages.CarsListed);
        }

        public IDataResult<List<CarDetailDto>> GetCarDetailDtos()
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetails(), Messages.CarsListed);
        }

        public IDataResult<List<Car>> GetCarsByBrandId(int id)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(p => p.BrandId == id));
        }

        public IDataResult<List<Car>> GetCarsByColorId(int id)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(p => p.ColorId == id));
        }


        [ValidationAspect(typeof(CarValidator))]
        [CacheRemoveAspect("ICarService.Get")]
        public IResult Update(Car car)
        {
            //if (car.Description.Length<2 && car.DailyPrice < 0)
            //{
            //    return new ErrorResult(Messages.CarNameInvalid);
            //}
            _carDal.Update(car);
            return new SuccessResult(Messages.CarUpdated);
        }

        public IDataResult<List<CarDetailDto>> GetCarDetailsByBrandId(int brandId)
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetailsByBrandId(brandId));
        }

        public IDataResult<List<CarDetailDto>> GetCarDetailsByColorId(int colorId)
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetailsByColorId(colorId));
        }

        public IDataResult<List<CarDetailDto>> GetCarDetailsByCarId(int carId)
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetailsByCarId(carId));
        }
        public IDataResult<CarDetailDto> GetCarDetailsById(int id)
        {
            return new SuccessDataResult<CarDetailDto>(_carDal.GetCarDetailsById(id));
        }
        public IDataResult<List<Car>> GetById(int id)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.Id == id));
        }


        public IDataResult<List<CarDetailDto>> GetNotRentedCarsByLocationId(int locationId, bool IsRented)
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetNotRentedCarsByLocationId(locationId, false));
        }

        public IDataResult<List<CarDetailDto>> GetCarsNotRentedByLocationName(string locationName, bool IsRented)
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarsNotRentedByLocationName(locationName, false));
        }

        public void CarRented(int carId)
        {
            Car car = new Car();
            car = _carDal.Get(c => c.Id == carId);
            car.IsRented = true;

            _carDal.Update(car);
        }

        public IDataResult<int> UpdatePriceByAction(int carId, string action)
        {
            // 1️⃣ Aracı database’den al
            var car = _carDal.Get(c => c.Id == carId);
            if (car == null)
                return new ErrorDataResult<int>(0, "Car not found");

            // 2️⃣ Fiyat güncelleme adımı (step)
            int step = 100; // 100 birim veya istersen %5 yapabilirsin
            switch (action.ToLower())
            {
                case "increase":
                    car.DailyPrice += step;
                    break;
                case "decrease":
                    car.DailyPrice = Math.Max(car.DailyPrice - step, 0); // negatif olmasın
                    break;
                case "same":
                    // değişiklik yok
                    break;
                default:
                    return new ErrorDataResult<int>(car.DailyPrice, "Unknown action");
            }

            // 3️⃣ Güncellenmiş fiyatı database’e yaz
            _carDal.Update(car);

            // 4️⃣ Sonuç dön
            return new SuccessDataResult<int>(car.DailyPrice, $"Price {action} applied successfully");
        }

        public IDataResult<List<CarDetailDto>> GetCarsByGearAndFuelFilters(List<int> fuelIds, List<int> gearIds, bool isRented, string locationName)
        {
            return new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarsByGearAndFuelFilters(fuelIds, gearIds, isRented, locationName));
        }

    }
}
