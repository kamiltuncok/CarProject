using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Business.Abstract
{
    public interface ISegmentService : IGenericService<Segment>
    {
    
        Task<IDataResult<List<Segment>>> GetAllAsync();
        Task<IDataResult<Segment>> GetByIdAsync(int id);
        Task<IResult> AddAsync(Segment entity);
        Task<IResult> UpdateAsync(Segment entity);
        Task<IResult> DeleteAsync(int id);
    }

}

