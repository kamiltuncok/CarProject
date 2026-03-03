using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace DataAccess.Abstract
{
   public interface ICustomerDal:IEntityRepository<Customer>
    {
        CustomerDetailDto GetCustomerDetailById(int id);
        List<CustomerDetailDto> GetCustomerDetails();
        Task<CustomerDetailDto> GetCustomerDetailByIdAsync(int id);
        Task<List<CustomerDetailDto>> GetCustomerDetailsAsync();
    }
}
