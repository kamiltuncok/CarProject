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
using System.Linq;
using Core.Aspects.Autofac.Transaction;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        IRentalDal _rentalDal;
        ICarDal _carDal;
        ICustomerDal _customerDal;
        ICorporateProfileDal _corporateProfileDal;
        IIndividualCustomerDal _individualCustomerDal;

        public RentalManager(IRentalDal rentalDal, ICarDal carDal, ICustomerDal customerDal, ICorporateProfileDal corporateProfileDal, IIndividualCustomerDal individualCustomerDal)
        {
            _rentalDal = rentalDal;
            _carDal = carDal;
            _customerDal = customerDal;
            _corporateProfileDal = corporateProfileDal;
            _individualCustomerDal = individualCustomerDal;
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

        // --- DomainV2 Production-Safe Methods ---

        public IResult CheckCarAvailability(int carId, DateTime startDate, DateTime endDate)
        {
            // 1. Basic Date Validation
            if (startDate >= endDate) return new ErrorResult("Alış tarihi, iade tarihinden önce olmalıdır.");

            // 2. Strict Overlap Check
            var overlappingRentals = _rentalDal.GetAll(r =>
                r.CarId == carId &&
                (r.Status == RentalStatus.Active || r.Status == RentalStatus.Pending) &&
                r.StartDate < endDate && 
                r.EndDate > startDate);

            if (overlappingRentals != null && overlappingRentals.Any())
            {
                return new ErrorResult("Araç belirtilen tarihler arasında zaten kiralanmış.");
            }

            return new SuccessResult("Araç uygun.");
        }

        [TransactionScopeAspect]
        public IDataResult<RentalResponseDto> CreateRental(RentalCreateRequestDto request, int customerId)
        {
            // 1. Mandatory Validations
            if (request.StartDate >= request.EndDate)
                return new ErrorDataResult<RentalResponseDto>("Geçersiz tarih aralığı.");

            // 2. Strict Availability Check
            var availabilityResult = CheckCarAvailability(request.CarId, request.StartDate, request.EndDate);
            if (!availabilityResult.Success)
                return new ErrorDataResult<RentalResponseDto>(availabilityResult.Message);

            // 3. Fetch Car & Calculate Prices Server-Side (Zero Trust Frontend)
            var car = _carDal.Get(c => c.Id == request.CarId);
            if (car == null)
                return new ErrorDataResult<RentalResponseDto>("Araç bulunamadı.");

            if (car.CurrentLocationId != request.StartLocationId || car.Status == CarStatus.Maintenance)
                return new ErrorDataResult<RentalResponseDto>("Araç belirtilen lokasyonda değil veya bakımsız durumda.");

            int days = Math.Max(1, (int)(request.EndDate - request.StartDate).TotalDays);
            decimal totalPrice = car.DailyPrice * days;

            // 4. Create Rental Entity (Pending Payment Simulation)
            var rental = new Rental
            {
                CarId = car.Id,
                CustomerId = customerId,
                StartLocationId = request.StartLocationId,
                EndLocationId = request.EndLocationId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                RentedDailyPrice = car.DailyPrice,
                TotalPrice = totalPrice,
                DepositAmount = car.Deposit,
                Status = RentalStatus.Active,
                DepositStatus = DepositStatus.Blocked
            };

            // This hits the DB and assigns an ID
            _rentalDal.Add(rental);

            // 5. Update Car Status safely (Concurrency Token checks RowVersion here)
            car.Status = CarStatus.Rented;
            _carDal.Update(car); 

            // If we reach here without exceptions from EF Core (DbUpdateConcurrencyException),
            // the TransactionScopeAspect will automatically commit.
            
            return new SuccessDataResult<RentalResponseDto>(
                new RentalResponseDto { RentalId = rental.Id, TotalPrice = totalPrice },
                "Kiralama işlemi ve (simüle edilen) ödeme başarıyla gerçekleşti."
            );
        }

        [TransactionScopeAspect]
        public IDataResult<RentalResponseDto> CreateGuestRental(GuestRentalCreateRequestDto request)
        {
            // 1. Mandatory Validations
            if (request.StartDate >= request.EndDate)
                return new ErrorDataResult<RentalResponseDto>("Geçersiz tarih aralığı.");

            // 2. Strict Availability Check
            var availabilityResult = CheckCarAvailability(request.CarId, request.StartDate, request.EndDate);
            if (!availabilityResult.Success)
                return new ErrorDataResult<RentalResponseDto>(availabilityResult.Message);

            // 3. Fetch Car
            var car = _carDal.Get(c => c.Id == request.CarId);
            if (car == null)
                return new ErrorDataResult<RentalResponseDto>("Araç bulunamadı.");

            if (car.CurrentLocationId != request.StartLocationId || car.Status == CarStatus.Maintenance)
                return new ErrorDataResult<RentalResponseDto>("Araç belirtilen lokasyonda değil veya bakımsız durumda.");

            int days = Math.Max(1, (int)(request.EndDate - request.StartDate).TotalDays);
            decimal totalPrice = car.DailyPrice * days;

            // 4. Create the Guest Customer
            var customer = new Customer
            {
                CustomerType = request.CustomerType,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                UserId = null, // Mark as unauthenticated guest
                CreatedDate = DateTime.Now
            };

            _customerDal.Add(customer); // Entity Framework sets customer.Id automatically

            // 5. Create CorporateProfile if applicable
            if (request.CustomerType == CustomerType.Corporate)
            {
                var corporateProfile = new CorporateProfile
                {
                    CustomerId = customer.Id,
                    CompanyName = request.CompanyName,
                    TaxNumber = request.TaxNumber
                };
                _corporateProfileDal.Add(corporateProfile);
            }
            else if (request.CustomerType == CustomerType.Individual)
            {
                var individualCustomer = new IndividualCustomer
                {
                    CustomerId = customer.Id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    IdentityNumber = request.IdentityNumber,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address
                };
                _individualCustomerDal.Add(individualCustomer);
            }

            // 6. Create Rental Entity referencing the newly created Customer ID (Pending Payment Simulation)
            var rental = new Rental
            {
                CarId = car.Id,
                CustomerId = customer.Id,
                StartLocationId = request.StartLocationId,
                EndLocationId = request.EndLocationId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                RentedDailyPrice = car.DailyPrice,
                TotalPrice = totalPrice,
                DepositAmount = car.Deposit,
                Status = RentalStatus.Active,
                DepositStatus = DepositStatus.Blocked
            };

            _rentalDal.Add(rental);

            // 7. Update Car Status safely (Concurrency Token checks RowVersion here)
            car.Status = CarStatus.Rented;
            _carDal.Update(car); 

            // TransactionScopeAspect auto-commits if everything succeeds.
            return new SuccessDataResult<RentalResponseDto>(
                new RentalResponseDto { RentalId = rental.Id, TotalPrice = totalPrice },
                "Kayıtsız misafir olarak kiralama işlemi ve (simüle edilen) ödeme başarıyla gerçekleşti."
            );
        }
    }
}
