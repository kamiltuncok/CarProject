using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
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

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        IRentalDal _rentalDal;
        ICarDal _carDal;

        public RentalManager(IRentalDal rentalDal, ICarDal carDal)
        {
            _rentalDal = rentalDal;
            _carDal = carDal;
        }

        [ValidationAspect(typeof(RentalValidator))]
        public IResult Add(Rental rental)
        {
            ValidationTool.Validate(new RentalValidator(), rental);

            // Set default status on add
            rental.Status = RentalStatus.Active;
            rental.DepositStatus = DepositStatus.Blocked;
            rental.DepositDeductedAmount = 0;

            _rentalDal.Add(rental);
            return new SuccessResult(Messages.RentalAdded);
        }

        public IResult Delete(Rental rental)
        {
            _rentalDal.Delete(rental);
            return new SuccessResult(Messages.RentalDeleted);
        }

        public IDataResult<List<Rental>> GetAll()
        {
            return new SuccessDataResult<List<Rental>>(_rentalDal.GetAll(), Messages.RentalListed);
        }

        public IDataResult<List<Rental>> GetById(int rentalid)
        {
            return new SuccessDataResult<List<Rental>>(_rentalDal.GetAll(r => r.Id == rentalid), Messages.RentalListed);
        }

        public IResult Update(Rental rental)
        {
            _rentalDal.Update(rental);
            return new SuccessResult(Messages.RetalUpdated);
        }

        public IDataResult<List<Rental>> GetRentalsByCarId(int carId)
        {
            var rentals = _rentalDal.GetAll(r => r.CarId == carId && r.Status == RentalStatus.Completed)
                ?? new List<Rental>();
            return new SuccessDataResult<List<Rental>>(rentals, Messages.RentalListed);
        }

        public IDataResult<List<RentalDetailDto>> GetRentalDetailsByUserId(int userId, CustomerType customerType)
        {
            var result = _rentalDal.GetRentalDetailsByUserId(userId, customerType);
            return new SuccessDataResult<List<RentalDetailDto>>(result);
        }

        public IDataResult<List<RentalDetailDto>> GetRentalDetailsByLocationName(string locationName)
        {
            var result = _rentalDal.GetRentalDetailsByLocationName(locationName);
            return new SuccessDataResult<List<RentalDetailDto>>(result, Messages.RentalListed);
        }

        public IResult AddBulk(List<Rental> rentals)
        {
            try
            {
                _rentalDal.AddRange(rentals);
                return new SuccessResult($"{rentals.Count} rental eklendi");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Hata: {ex.Message}");
            }
        }

        public IDataResult<List<Rental>> GetRentalsByStartDate(DateTime startDate)
        {
            var rentals = _rentalDal.GetAll(r => r.StartDate.Date == startDate.Date);
            return new SuccessDataResult<List<Rental>>(rentals, Messages.RentalListed);
        }

        public IDataResult<List<RentalDetailDto>> GetRentalsByEmail(string email)
        {
            var result = _rentalDal.GetRentalsByEmail(email);
            return new SuccessDataResult<List<RentalDetailDto>>(result, Messages.RentalListed);
        }

        public IDataResult<List<RentalDetailDto>> GetRentalsByName(string name)
        {
            var result = _rentalDal.GetRentalsByName(name);
            return new SuccessDataResult<List<RentalDetailDto>>(result, Messages.RentalListed);
        }

        public IDataResult<List<RentalDetailDto>> GetRentalsByDateRange(DateTime startDate, DateTime endDate)
        {
            var result = _rentalDal.GetRentalsByDateRange(startDate, endDate);
            return new SuccessDataResult<List<RentalDetailDto>>(result, Messages.RentalListed);
        }

        public IResult MarkAsReturned(int rentalId)
        {
            var rental = _rentalDal.Get(r => r.Id == rentalId);
            if (rental == null)
                return new ErrorResult("Kiralama bulunamadı.");

            rental.Status = RentalStatus.Completed;
            rental.DepositStatus = DepositStatus.Refunded;
            rental.DepositRefundedDate = DateTime.Now;
            _rentalDal.Update(rental);

            // Update car status to Available
            var car = _carDal.Get(c => c.Id == rental.CarId);
            if (car != null)
            {
                car.Status = CarStatus.Available;
                _carDal.Update(car);
            }

            return new SuccessResult("Araç teslim alındı.");
        }

        public IResult DeleteAndFreeCar(int rentalId)
        {
            var rental = _rentalDal.Get(r => r.Id == rentalId);
            if (rental == null)
                return new ErrorResult("Kiralama bulunamadı.");

            int carId = rental.CarId;
            _rentalDal.Delete(rental);

            var car = _carDal.Get(c => c.Id == carId);
            if (car != null)
            {
                car.Status = CarStatus.Available;
                _carDal.Update(car);
            }

            return new SuccessResult("Kiralama silindi.");
        }
    }
}
