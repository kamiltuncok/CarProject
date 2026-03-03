using Core.Utilities;
using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Business.Abstract
{
   public interface IColorService
    {
        IDataResult<List<Color>> GetAll();
        IDataResult<List<Color>> GetById(int id);
        IResult Add(Color color);
        IResult Delete(Color color);
        IResult Update(Color color);
    
        Task<IDataResult<List<Color>>> GetAllAsync();
        Task<IDataResult<Color>> GetByIdAsync(int id);
        Task<IResult> AddAsync(Color entity);
        Task<IResult> UpdateAsync(Color entity);
        Task<IResult> DeleteAsync(int id);
    }

}
