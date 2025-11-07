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
using System.Text;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        IRentalDal _rentalDal;
        public RentalManager(IRentalDal rentalDal)
        {
            _rentalDal = rentalDal;
        }
        [ValidationAspect(typeof(RentalValidator))]
        public IResult Add(Rental rental)
        {

            ValidationTool.Validate(new RentalValidator(), rental);

            rental.isReturned = false;
            _rentalDal.Add(rental);
                return new SuccessResult(Messages.RentalAdded);
           
        }

        public IResult Delete(Rental rental)
        {
            _rentalDal.Delete(rental);
            return new SuccessResult(Messages.RentalDeleted);
        }

        //public IDataResult<List<RentalDetailDto>> GetRentalDetails()
        //{
        //    var result = _rentalDal.GetRentalDetails();
        //    return new SuccessDataResult<List<RentalDetailDto>>(result, Messages.rentalDetailsListed);
        //}

        public IDataResult<List<Rental>> GetAll()
        {
            return new SuccessDataResult<List<Rental>>(_rentalDal.GetAll(), Messages.RentalListed);
        }

        public IDataResult<List<Rental>> GetById(int rentalid)
        {
            return new SuccessDataResult<List<Rental>>(_rentalDal.GetAll(r=>r.RentalId==rentalid), Messages.RentalListed);
        }

        public IResult Update(Rental rental)
        {
            if (rental.ReturnDate == null)
            {
                return new ErrorResult(Messages.RentalNotAdded);
            }
            else
            {
                _rentalDal.Update(rental);
                return new SuccessResult(Messages.RetalUpdated);
            }
        }

        public IDataResult<List<Rental>> GetRentalsByCarId(int carId)
        {
            var rentals = _rentalDal.GetAll(r => r.CarId == carId && r.isReturned == true) ?? new List<Rental>();
            return new SuccessDataResult<List<Rental>>(rentals, Messages.RentalListed);
        }

        public IDataResult<List<RentalDetailDto>> GetRentalDetailsByUserId(int userId,CustomerType customerType)
        {
            var result = _rentalDal.GetRentalDetailsByUserId(userId,customerType);
            return new SuccessDataResult<List<RentalDetailDto>>(result);
        }

        public IDataResult<List<RentalDetailDto>> GetRentalDetailsByLocationName(string locationName)
        {
            var result = _rentalDal.GetRentalDetailsByLocationName(locationName);
            return new SuccessDataResult<List<RentalDetailDto>>(result, Messages.RentalListed);
        }
    }
}
