using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Business.Abstract
{
    public interface IFuelService : IGenericService<Fuel>
    {
    
        Task<IDataResult<List<Fuel>>> GetAllAsync();
        Task<IDataResult<Fuel>> GetByIdAsync(int id);
        Task<IResult> AddAsync(Fuel entity);
        Task<IResult> UpdateAsync(Fuel entity);
        Task<IResult> DeleteAsync(int id);
    }

}

