using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class LocationUserRoleManager : ILocationUserRoleService
    {
        private ILocationUserRoleDal _locationUserRoleDal;

        public LocationUserRoleManager(ILocationUserRoleDal locationUserRoleDal)
        {
            _locationUserRoleDal = locationUserRoleDal;
        }

        public IResult Add(LocationUserRole locationUserRole)
        {
            _locationUserRoleDal.Add(locationUserRole);
            return new SuccessResult("Location Role Assigned.");
        }

        public IResult Delete(LocationUserRole locationUserRole)
        {
            _locationUserRoleDal.Delete(locationUserRole);
            return new SuccessResult("Location Role Deleted.");
        }

        public IDataResult<List<LocationUserRole>> GetAll()
        {
            return new SuccessDataResult<List<LocationUserRole>>(_locationUserRoleDal.GetAll());
        }

        public IDataResult<LocationUserRole> GetById(int id)
        {
            return new SuccessDataResult<LocationUserRole>(_locationUserRoleDal.Get(l => l.Id == id));
        }

        public IDataResult<List<LocationUserRole>> GetByLocationId(int locationId)
        {
            return new SuccessDataResult<List<LocationUserRole>>(_locationUserRoleDal.GetAll(l => l.LocationId == locationId));
        }

        public IDataResult<LocationUserRole> GetByUserAndLocation(int userId, int locationId)
        {
            return new SuccessDataResult<LocationUserRole>(_locationUserRoleDal.Get(l => l.UserId == userId && l.LocationId == locationId));
        }

        public IDataResult<List<LocationUserRole>> GetByUserId(int userId)
        {
            return new SuccessDataResult<List<LocationUserRole>>(_locationUserRoleDal.GetAll(l => l.UserId == userId));
        }

        public IResult Update(LocationUserRole locationUserRole)
        {
            _locationUserRoleDal.Update(locationUserRole);
            return new SuccessResult("Location Role Updated.");
        }
    }
}
