using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICorporateCustomerService : IGenericService<CorporateCustomer>
    {
        IDataResult<CorporateCustomer> AddCustomer(CorporateCustomer entity);
            Task<IDataResult<List<CorporateCustomer>>> GetAllAsync();
        Task<IDataResult<CorporateCustomer>> GetByIdAsync(int id);
        Task<IResult> AddAsync(CorporateCustomer customer);
        Task<IResult> UpdateAsync(CorporateCustomer customer);
        Task<IResult> DeleteAsync(CorporateCustomer customer);
    }
}

