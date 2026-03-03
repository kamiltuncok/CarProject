using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IIndividualCustomerService : IGenericService<IndividualCustomer>
    {
        IDataResult<IndividualCustomer> AddCustomer(IndividualCustomer entity);
            Task<IDataResult<List<IndividualCustomer>>> GetAllAsync();
        Task<IDataResult<IndividualCustomer>> GetByIdAsync(int id);
        Task<IResult> AddAsync(IndividualCustomer customer);
        Task<IResult> UpdateAsync(IndividualCustomer customer);
        Task<IResult> DeleteAsync(IndividualCustomer customer);
    }
}

