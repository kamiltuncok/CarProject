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
using DataAccess.Concrete.EntityFramework;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        IRentalDal _rentalDal;
        ICarDal _carDal;
        ICustomerDal _customerDal;

        public RentalManager(IRentalDal rentalDal, ICarDal carDal, ICustomerDal customerDal)
        {
            _rentalDal = rentalDal;
            _carDal = carDal;
            _customerDal = customerDal;
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

        public IDataResult<List<RentalDetailDto>> GetRentalDetailsByUserId(int userId)
        {
            var result = _rentalDal.GetRentalDetailsByUserId(userId);
            return new SuccessDataResult<List<RentalDetailDto>>(result);
        }

        public IDataResult<List<RentalDetailDto>> GetRentalDetailsByLocationName(string locationName)
        {
            var result = _rentalDal.GetRentalDetailsByLocationName(locationName);
            return new SuccessDataResult<List<RentalDetailDto>>(result, Messages.RentalListed);
        }

        public IDataResult<List<RentalDetailDto>> GetRentalsByManagerLocation(int userId)
        {
            // Instead of injecting ILocationUserRoleService, query through a fresh context safely
            var managerLocations = new List<LocationUserRole>();
            using (var context = new RentACarContext())
            {
                managerLocations = context.LocationUserRoles.Where(loc => loc.UserId == userId).ToList();
            
                if (!managerLocations.Any())
                    return new ErrorDataResult<List<RentalDetailDto>>("User is not managing any locations.");

                var allRentals = new List<RentalDetailDto>();

                foreach (var locRole in managerLocations)
                {
                    var location = context.Locations.FirstOrDefault(l => l.Id == locRole.LocationId);
                    if (location != null)
                    {
                        var rentals = _rentalDal.GetRentalDetailsByLocationName(location.LocationName);
                        if (rentals != null && rentals.Any())
                            allRentals.AddRange(rentals);
                    }
                }
                return new SuccessDataResult<List<RentalDetailDto>>(allRentals.GroupBy(r => r.Id).Select(g => g.First()).ToList(), Messages.RentalListed);
            }
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

            return new SuccessResult("Araç teslim edildi.");
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
        public IDataResult<RentalResponseDto> CreateRental(RentalCreateRequestDto request, int userId)
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

            // 4. Resolve CustomerId from UserId
            Customer customer;
            using (var context = new RentACarContext())
            {
                var userRecord = context.Users.FirstOrDefault(u => u.Id == userId);
                if (userRecord == null) return new ErrorDataResult<RentalResponseDto>("Kullanıcı bulunamadı.");
                
                customer = _customerDal.Get(c => c.Id == userRecord.CustomerId);
            }
            
            if (customer == null)
            {
                return new ErrorDataResult<RentalResponseDto>("Kullanıcıya ait müşteri kaydı bulunamadı.");
            }

            // 5. Create Rental Entity (Pending Payment Simulation)
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

            // 4. Create the Guest Customer via Polymorphism
            Customer customer;
            
            if (!string.IsNullOrEmpty(request.CompanyName))
            {
                customer = new CorporateCustomer
                {
                    CompanyName = request.CompanyName,
                    TaxNumber = request.TaxNumber,
                    PhoneNumber = request.PhoneNumber,
                    CreatedDate = DateTime.Now
                };
            }
            else
            {
                customer = new IndividualCustomer
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    IdentityNumber = request.IdentityNumber,
                    PhoneNumber = request.PhoneNumber,
                    CreatedDate = DateTime.Now
                };
            }

            _customerDal.Add(customer); // Entity Framework sets customer.Id automatically via TPT

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

        [TransactionScopeAspect]
        public IResult CollectDeposit(int rentalId, int userId)
        {
            var rental = _rentalDal.Get(r => r.Id == rentalId);
            if (rental == null) return new ErrorResult("Kiralama bulunamadı.");

            if (rental.Status != RentalStatus.Active)
                return new ErrorResult("Sadece aktif kiralamalar için depozito tahsil edilebilir.");
            if (rental.DepositStatus != DepositStatus.Blocked)
                return new ErrorResult("Depozito zaten tahsil edilmiş veya bekleyen bir depozito yok.");

            using (var context = new RentACarContext())
            {
                var isManager = context.LocationUserRoles.Any(l => l.UserId == userId && l.LocationId == rental.StartLocationId);
                var isAdmin = context.UserOperationClaims.Any(u => u.UserId == userId && context.OperationClaims.Any(c => c.Id == u.OperationClaimId && c.Name == "admin"));
                if (!isManager && !isAdmin) return new ErrorResult("Bu işlem için yetkiniz yok (Alış lokasyonu yöneticisi değilsiniz).");
            }

            rental.DepositStatus = DepositStatus.Charged;
            _rentalDal.Update(rental);
            
            return new SuccessResult("Depozito başarıyla tahsil edildi.");
        }

        [TransactionScopeAspect]
        public IResult DeliverVehicle(int rentalId, int userId)
        {
            var rental = _rentalDal.Get(r => r.Id == rentalId);
            if (rental == null) return new ErrorResult("Kiralama bulunamadı.");

            if (rental.Status != RentalStatus.Active)
                return new ErrorResult("Sadece aktif kiralamalar teslim alınabilir.");

            using (var context = new RentACarContext())
            {
                var isManager = context.LocationUserRoles.Any(l => l.UserId == userId && l.LocationId == rental.EndLocationId);
                var isAdmin = context.UserOperationClaims.Any(u => u.UserId == userId && context.OperationClaims.Any(c => c.Id == u.OperationClaimId && c.Name == "admin"));
                if (!isManager && !isAdmin) return new ErrorResult("Bu işlem için yetkiniz yok (Bırakış lokasyonu yöneticisi değilsiniz).");
            }

            rental.Status = RentalStatus.Completed;
            rental.DepositStatus = DepositStatus.Refunded;
            rental.DepositRefundedDate = DateTime.Now;
            _rentalDal.Update(rental);

            var car = _carDal.Get(c => c.Id == rental.CarId);
            if (car != null)
            {
                car.Status = CarStatus.Available;
                car.CurrentLocationId = rental.EndLocationId;
                _carDal.Update(car); // row version optimistic concurrency checks handle race conditions automatically.
            }

            return new SuccessResult("Araç başarıyla teslim edildi.");
        }

        [TransactionScopeAspect]
        public IResult CancelRental(int rentalId, int userId)
        {
            var rental = _rentalDal.Get(r => r.Id == rentalId);
            if (rental == null) return new ErrorResult("Kiralama bulunamadı.");

            if (rental.Status != RentalStatus.Active && rental.Status != RentalStatus.Pending)
                return new ErrorResult("Sadece aktif veya bekleyen kiralamalar iptal edilebilir.");

            using (var context = new RentACarContext())
            {
                var isManager = context.LocationUserRoles.Any(l => l.UserId == userId && (l.LocationId == rental.StartLocationId || l.LocationId == rental.EndLocationId));
                var isAdmin = context.UserOperationClaims.Any(u => u.UserId == userId && context.OperationClaims.Any(c => c.Id == u.OperationClaimId && c.Name == "admin"));
                if (!isManager && !isAdmin) return new ErrorResult("Bu işlem için yetkiniz yok (Alış veya Bırakış lokasyonu yöneticisi değilsiniz).");
            }

            rental.Status = RentalStatus.Cancelled;
            rental.DepositStatus = DepositStatus.Refunded;
            rental.DepositRefundedDate = DateTime.Now;
            _rentalDal.Update(rental);

            var car = _carDal.Get(c => c.Id == rental.CarId);
            if (car != null)
            {
                car.Status = CarStatus.Available;
                _carDal.Update(car);
            }

            return new SuccessResult("Kiralama başarıyla iptal edildi.");
        }
    }
}
