using Core.Utilities.Results;
using Entities.DTOs;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface ILocationManagerUserService
    {
        IResult AddLocationManager(LocationManagerAddDto locationManagerAddDto);
        IResult UpdateLocationManager(LocationManagerUpdateDto locationManagerUpdateDto);
        IResult RevokeLocationManager(int userId, int locationId);
        IDataResult<List<LocationManagerDto>> GetLocationManagers();
    }
}
