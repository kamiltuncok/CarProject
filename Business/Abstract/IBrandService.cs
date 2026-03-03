using Core.Utilities;
using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Business.Abstract
{
   public interface IBrandService
    {
        IDataResult<List<Brand>> GetAll();
        IDataResult<List<Brand>> GetById(int id);
        IResult Add(Brand brand);
        IResult Delete(Brand brand);
        IResult Update(Brand brand);
    
        Task<IDataResult<List<Brand>>> GetAllAsync();
        Task<IDataResult<Brand>> GetByIdAsync(int id);
        Task<IResult> AddAsync(Brand entity);
        Task<IResult> UpdateAsync(Brand entity);
        Task<IResult> DeleteAsync(int id);
    }

}
