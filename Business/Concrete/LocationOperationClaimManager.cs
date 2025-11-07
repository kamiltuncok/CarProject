using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class LocationOperationClaimManager : ILocationOperationClaimService
    {
        ILocationOperationClaimDal _locationOperationClaimDal;
        private readonly IRentalService _rentalService;

        public LocationOperationClaimManager(ILocationOperationClaimDal locationOperationClaimDal,
            IRentalService rentalService)
        {
            _locationOperationClaimDal = locationOperationClaimDal;
            _rentalService = rentalService;
        }

        public IResult Add(LocationOperationClaim entity)
        {
            _locationOperationClaimDal.Add(entity);
            return new SuccessResult();
        }

        public IResult Delete(LocationOperationClaim entity)
        {
            _locationOperationClaimDal.Delete(entity);
            return new SuccessResult();
        }

        public IDataResult<List<LocationOperationClaim>> GetAll()
        {
            return new SuccessDataResult<List<LocationOperationClaim>>(_locationOperationClaimDal.GetAll());
        }

        public IDataResult<LocationOperationClaim> GetById(int id)
        {
            return new SuccessDataResult<LocationOperationClaim>(_locationOperationClaimDal.Get(l => l.LocationOperationClaimId == id));
        }

        public IDataResult<List<LocationOperationClaim>> GetListById(int id)
        {
            return new SuccessDataResult<List<LocationOperationClaim>>(_locationOperationClaimDal.GetAll(l => l.LocationOperationClaimId == id));
        }

        public IResult Update(LocationOperationClaim entity)
        {
            _locationOperationClaimDal.Update(entity);
            return new SuccessResult();
        }

        public IDataResult<List<LocationOperationClaim>> GetByUserId(int userId)
        {
            var result = _locationOperationClaimDal.GetAll(loc => loc.UserId == userId);
            return new SuccessDataResult<List<LocationOperationClaim>>(result);
        }

        public IDataResult<List<LocationOperationClaim>> GetByLocationId(int locationId)
        {
            var result = _locationOperationClaimDal.GetAll(loc => loc.LocationId == locationId);
            return new SuccessDataResult<List<LocationOperationClaim>>(result);
        }

        public IDataResult<LocationOperationClaim> GetByUserAndLocation(int userId, int locationId)
        {
            var result = _locationOperationClaimDal.Get(loc => loc.UserId == userId && loc.LocationId == locationId);
            return new SuccessDataResult<LocationOperationClaim>(result);
        }

        public IDataResult<bool> IsUserLocationManager(int userId, int locationId)
        {
            var result = _locationOperationClaimDal.Get(loc => loc.UserId == userId && loc.LocationId == locationId);
            return new SuccessDataResult<bool>(result != null, result != null ? "Kullanıcı lokasyon yöneticisi" : "Kullanıcı lokasyon yöneticisi değil");
        }

        public IDataResult<List<RentalDetailDto>> GetRentalsByManagerLocation(int userId)
        {
            // 1. Kullanıcının yöneticisi olduğu lokasyonları bul
            var managerLocations = _locationOperationClaimDal.GetAll(loc => loc.UserId == userId);

            if (!managerLocations.Any())
                return new ErrorDataResult<List<RentalDetailDto>>();

            // 2. Bu lokasyonların isimlerini al (Location tablosundan)
            var allRentals = new List<RentalDetailDto>();

            using (var context = new RentACarContext())
            {
                foreach (var managerLocation in managerLocations)
                {
                    // Location tablosundan lokasyon ismini al
                    var location = context.Locations.FirstOrDefault(l => l.LocationId == managerLocation.LocationId);
                    if (location != null)
                    {
                        // StartLocation ile rental'ları getir
                        var rentals = _rentalService.GetRentalDetailsByLocationName(location.LocationName).Data;
                        if (rentals != null && rentals.Any())
                            allRentals.AddRange(rentals);
                    }
                }
            }

            return new SuccessDataResult<List<RentalDetailDto>>(allRentals, Messages.RentalListed);
        }

    }
}
