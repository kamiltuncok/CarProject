using Core.Utilities;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
   public interface ICustomerService
    {
        IDataResult<List<Customer>> GetAll();
        Task<IDataResult<List<Customer>>> GetAllAsync();
        IDataResult<Customer> GetById(int customerid);
        Task<IDataResult<Customer>> GetByIdAsync(int customerid);
        IDataResult<CustomerDetailDto> GetCustomerDetailById(int id);
        Task<IDataResult<CustomerDetailDto>> GetCustomerDetailByIdAsync(int id);
        IDataResult<List<CustomerDetailDto>> GetCustomerDetails();
        Task<IDataResult<List<CustomerDetailDto>>> GetCustomerDetailsAsync();
        IResult Add(Customer customer);
        Task<IResult> AddAsync(Customer customer);
        IResult Delete(Customer customer);
        Task<IResult> DeleteAsync(Customer customer);
        IResult Update(Customer customer);
        Task<IResult> UpdateAsync(Customer customer);

    }
}

