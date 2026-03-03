using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Business.Abstract
{
    public interface IGearService : IGenericService<Gear>
    {
    
        Task<IDataResult<List<Gear>>> GetAllAsync();
        Task<IDataResult<Gear>> GetByIdAsync(int id);
        Task<IResult> AddAsync(Gear entity);
        Task<IResult> UpdateAsync(Gear entity);
        Task<IResult> DeleteAsync(int id);
    }

}

