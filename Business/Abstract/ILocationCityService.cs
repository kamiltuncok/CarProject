using Core.Utilities.Results;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Abstract
{
    public interface ILocationCityService : IGenericService<LocationCity>
    {
        IDataResult<List<LocationCity>> GetAll();
        IDataResult<LocationCity> GetById(int id);
    }
}
