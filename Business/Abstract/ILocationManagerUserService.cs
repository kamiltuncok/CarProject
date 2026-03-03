using Core.Utilities.Results;
using Entities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ILocationManagerUserService
    {
        IResult AddLocationManager(LocationManagerAddDto locationManagerAddDto);
        IResult UpdateLocationManager(LocationManagerUpdateDto locationManagerUpdateDto);
        IResult RevokeLocationManager(int userId, int locationId);
        IDataResult<List<LocationManagerDto>> GetLocationManagers();

        Task<IResult> AddLocationManagerAsync(LocationManagerAddDto locationManagerAddDto);
        Task<IResult> UpdateLocationManagerAsync(LocationManagerUpdateDto locationManagerUpdateDto);
        Task<IResult> RevokeLocationManagerAsync(int userId, int locationId);
        Task<IDataResult<List<LocationManagerDto>>> GetLocationManagersAsync();
    }
}
