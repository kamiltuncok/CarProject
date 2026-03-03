using Core.Utilities.Results;
using Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ILocationCityService : IGenericService<LocationCity>
    {
    
        Task<IDataResult<List<LocationCity>>> GetAllAsync();
        Task<IDataResult<LocationCity>> GetByIdAsync(int id);
        Task<IResult> AddAsync(LocationCity entity);
        Task<IResult> UpdateAsync(LocationCity entity);
        Task<IResult> DeleteAsync(int id);
    }

}
