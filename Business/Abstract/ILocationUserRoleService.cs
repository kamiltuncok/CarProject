using Core.Utilities.Results;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface ILocationUserRoleService
    {
        IResult Add(LocationUserRole locationUserRole);
        IResult Update(LocationUserRole locationUserRole);
        IResult Delete(LocationUserRole locationUserRole);
        IDataResult<List<LocationUserRole>> GetAll();
        IDataResult<LocationUserRole> GetById(int id);
        IDataResult<List<LocationUserRole>> GetByUserId(int userId);
        IDataResult<List<LocationUserRole>> GetByLocationId(int locationId);
        IDataResult<LocationUserRole> GetByUserAndLocation(int userId, int locationId);
    }
}
