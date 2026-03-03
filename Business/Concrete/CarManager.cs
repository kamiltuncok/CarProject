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
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

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
            car.Status = CarStatus.Available;
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
        public IDataResult<CarDetailDto> GetCarDetailsById(int id)
        {
            return new SuccessDataResult<CarDetailDto>(_carDal.GetCarDetailsById(id));
        }
        public IDataResult<List<Car>> GetById(int id)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(c => c.Id == id));
        }

        public IDataResult<List<CarDetailDto>> GetAvailableCars(CarAvailabilityFilterDto filter)
        {
            var result = _carDal.GetAvailableCars(filter);
            return new SuccessDataResult<List<CarDetailDto>>(result, "Müsait araçlar listelendi.");
        }

        public void CarRented(int carId)
        {
            Car car = new Car();
            car = _carDal.Get(c => c.Id == carId);
            car.Status = CarStatus.Rented;

            _carDal.Update(car);
        }

        public IDataResult<decimal> UpdatePriceByAction(int carId, string action)
        {
            // 1️⃣ Aracı database’den al
            var car = _carDal.Get(c => c.Id == carId);
            if (car == null)
                return new ErrorDataResult<decimal>(0, "Car not found");

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
                    return new ErrorDataResult<decimal>(car.DailyPrice, "Unknown action");
            }

            // 3️⃣ Güncellenmiş fiyatı database’e yaz
            _carDal.Update(car);

            // 4️⃣ Sonuç dön
            return new SuccessDataResult<decimal>(car.DailyPrice, $"Price {action} applied successfully");
        }

        public IDataResult<decimal> GetLowestPriceBySegmentId(int segmentId, bool isRented)
        {
            try
            {
                var lowestPrice = _carDal.GetLowestPriceBySegmentId(segmentId, isRented);
                return new SuccessDataResult<decimal>(lowestPrice, "En düşük fiyat getirildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<decimal>(0, "Fiyat getirilirken hata oluştu: " + ex.Message);
            }
        }

        public async Task<IDataResult<List<CarDetailDto>>> GetAvailableCarsAsync(CarAvailabilityFilterDto filter)
        {
            var result = await _carDal.GetAvailableCarsAsync(filter);
            return new SuccessDataResult<List<CarDetailDto>>(result, "Müsait araçlar listelendi.");
        }

        public async Task<IResult> CarRentedAsync(int carId)
        {
            var car = await _carDal.GetAsync(c => c.Id == carId);
            if (car != null)
            {
                car.Status = CarStatus.Rented;
                await _carDal.UpdateAsync(car);
                return new SuccessResult("Araç statusu kiralandı olarak güncellendi.");
            }
            return new ErrorResult("Araç bulunamadı.");
        }

        public async Task<IDataResult<decimal>> UpdatePriceByActionAsync(int carId, string action)
        {
            var car = await _carDal.GetAsync(c => c.Id == carId);
            if (car == null)
                return new ErrorDataResult<decimal>(0, "Car not found");

            int step = 100;
            switch (action.ToLower())
            {
                case "increase":
                    car.DailyPrice += step;
                    break;
                case "decrease":
                    car.DailyPrice = Math.Max(car.DailyPrice - step, 0);
                    break;
                case "same":
                    break;
                default:
                    return new ErrorDataResult<decimal>(car.DailyPrice, "Unknown action");
            }

            await _carDal.UpdateAsync(car);
            return new SuccessDataResult<decimal>(car.DailyPrice, $"Price {action} applied successfully");
        }

        public async Task<IDataResult<decimal>> GetLowestPriceBySegmentIdAsync(int segmentId, bool isRented)
        {
            try
            {
                var lowestPrice = await _carDal.GetLowestPriceBySegmentIdAsync(segmentId, isRented);
                return new SuccessDataResult<decimal>(lowestPrice, "En düşük fiyat getirildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<decimal>(0, "Fiyat getirilirken hata oluştu: " + ex.Message);
            }
        }

        [CacheAspect]
        [PerformanceAspect(5)]
        public async Task<IDataResult<List<Car>>> GetAllAsync()
        {
            var cars = await _carDal.GetAllAsync();
            return new SuccessDataResult<List<Car>>(cars, Messages.CarsListed);
        }

        public async Task<IDataResult<List<CarDetailDto>>> GetCarDetailDtosAsync()
        {
            var details = await _carDal.GetCarDetailsAsync();
            return new SuccessDataResult<List<CarDetailDto>>(details, Messages.CarsListed);
        }

        public async Task<IDataResult<List<CarDetailDto>>> GetCarDetailsByBrandIdAsync(int brandId)
        {
            var details = await _carDal.GetCarDetailsByBrandIdAsync(brandId);
            return new SuccessDataResult<List<CarDetailDto>>(details);
        }

        public async Task<IDataResult<List<CarDetailDto>>> GetCarDetailsByColorIdAsync(int colorId)
        {
            var details = await _carDal.GetCarDetailsByColorIdAsync(colorId);
            return new SuccessDataResult<List<CarDetailDto>>(details);
        }

        public async Task<IDataResult<CarDetailDto>> GetCarDetailsByIdAsync(int id)
        {
            var detail = await _carDal.GetCarDetailsByIdAsync(id);
            return new SuccessDataResult<CarDetailDto>(detail);
        }

        public async Task<IDataResult<List<Car>>> GetByIdAsync(int id)
        {
            return new SuccessDataResult<List<Car>>(await _carDal.GetAllAsync(c => c.Id == id));
        }

        [CacheRemoveAspect("ICarService.Get")]
        // Since we removed raw Entity param, we would ideally need a DTO validator. For now we just implement the mapping.
        public async Task<IResult> AddAsync(CarCreateDto carDto)
        {
            var car = new Car
            {
                BrandId = carDto.BrandId,
                ColorId = carDto.ColorId,
                CurrentLocationId = carDto.CurrentLocationId,
                ModelYear = carDto.ModelYear,
                DailyPrice = carDto.DailyPrice,
                Deposit = carDto.Deposit,
                PlateNumber = carDto.PlateNumber,
                KM = carDto.KM,
                Status = CarStatus.Available, // Enforce starting rule
                FuelId = carDto.FuelId,
                GearId = carDto.GearId,
                SegmentId = carDto.SegmentId,
                Description = carDto.Description
            };
            
            await _carDal.AddAsync(car);
            return new SuccessResult(Messages.CarAdded);
        }

        [CacheRemoveAspect("ICarService.Get")]
        public async Task<IResult> UpdateAsync(CarUpdateDto carDto)
        {
            var car = await _carDal.GetAsync(c => c.Id == carDto.Id);
            if (car == null)
            {
                return new ErrorResult("Kayıt bulunamadı.");
            }

            car.BrandId = carDto.BrandId;
            car.ColorId = carDto.ColorId;
            car.CurrentLocationId = carDto.CurrentLocationId;
            car.ModelYear = carDto.ModelYear;
            car.DailyPrice = carDto.DailyPrice;
            car.Deposit = carDto.Deposit;
            car.PlateNumber = carDto.PlateNumber;
            car.KM = carDto.KM;
            car.Status = carDto.Status;
            car.FuelId = carDto.FuelId;
            car.GearId = carDto.GearId;
            car.SegmentId = carDto.SegmentId;
            car.Description = carDto.Description;
            car.RowVersion = carDto.RowVersion;

            await _carDal.UpdateAsync(car);
            return new SuccessResult(Messages.CarUpdated);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var car = await _carDal.GetAsync(c => c.Id == id);
            if (car != null)
            {
                await _carDal.DeleteAsync(car);
                return new SuccessResult(Messages.CarDeleted);
            }
            return new ErrorResult("Silinecek araç bulunamadı.");
        }
    }
}
