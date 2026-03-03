using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Business.Abstract
{
    public interface ILocationService : IGenericService<Location>
    {
        Task<IDataResult<List<Location>>> GetByCityIdAsync(int locationCityId);
    
        Task<IDataResult<List<Location>>> GetAllAsync();
        Task<IDataResult<Location>> GetByIdAsync(int id);
        Task<IResult> AddAsync(Location entity);
        Task<IResult> UpdateAsync(Location entity);
        Task<IResult> DeleteAsync(int id);
    }

}
