using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ILocationOperationClaimService : IGenericService<LocationOperationClaim>
    {
        IDataResult<List<LocationOperationClaim>> GetByUserId(int userId);
        IDataResult<List<LocationOperationClaim>> GetByLocationId(int locationId);
        IDataResult<LocationOperationClaim> GetByUserAndLocation(int userId, int locationId);
        IDataResult<bool> IsUserLocationManager(int userId, int locationId);

        IDataResult<List<RentalDetailDto>> GetRentalsByManagerLocation(int userId);
    }
}
