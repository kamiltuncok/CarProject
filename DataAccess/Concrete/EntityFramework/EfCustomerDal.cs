using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfIndividualCustomerDal : EfEntityRepositoryBase<IndividualCustomer, RentACarContext>, IIndividualCustomerDal
    {
    }

    public class EfCustomerDal : EfEntityRepositoryBase<Customer, RentACarContext>, ICustomerDal
    {
        public CustomerDetailDto GetCustomerDetailById(int id)
        {
            using (var context = new RentACarContext())
            {
                var result = from c in context.Customers
                             where c.Id == id
                             select new CustomerDetailDto
                             {
                                 Id = c.Id,
                                 Email = c.Users.FirstOrDefault() != null ? c.Users.FirstOrDefault().Email : null,
                                 PhoneNumber = c.PhoneNumber,
                                 IdentityNumber = (c is IndividualCustomer) ? ((IndividualCustomer)c).IdentityNumber : null,
                                 UserId = c.Users.FirstOrDefault() != null ? (int?)c.Users.FirstOrDefault().Id : null,
                                 CreatedDate = c.CreatedDate,
                                 FirstName = (c is IndividualCustomer) ? ((IndividualCustomer)c).FirstName : null,
                                 LastName = (c is IndividualCustomer) ? ((IndividualCustomer)c).LastName : null,
                                 CompanyName = (c is CorporateCustomer) ? ((CorporateCustomer)c).CompanyName : null,
                                 TaxNumber = (c is CorporateCustomer) ? ((CorporateCustomer)c).TaxNumber : null
                             };
                return result.FirstOrDefault();
            }
        }

        public async Task<CustomerDetailDto> GetCustomerDetailByIdAsync(int id)
        {
            using (var context = new RentACarContext())
            {
                var result = from c in context.Customers
                             where c.Id == id
                             select new CustomerDetailDto
                             {
                                 Id = c.Id,
                                 Email = c.Users.FirstOrDefault() != null ? c.Users.FirstOrDefault().Email : null,
                                 PhoneNumber = c.PhoneNumber,
                                 IdentityNumber = (c is IndividualCustomer) ? ((IndividualCustomer)c).IdentityNumber : null,
                                 UserId = c.Users.FirstOrDefault() != null ? (int?)c.Users.FirstOrDefault().Id : null,
                                 CreatedDate = c.CreatedDate,
                                 FirstName = (c is IndividualCustomer) ? ((IndividualCustomer)c).FirstName : null,
                                 LastName = (c is IndividualCustomer) ? ((IndividualCustomer)c).LastName : null,
                                 CompanyName = (c is CorporateCustomer) ? ((CorporateCustomer)c).CompanyName : null,
                                 TaxNumber = (c is CorporateCustomer) ? ((CorporateCustomer)c).TaxNumber : null
                             };
                return await result.FirstOrDefaultAsync();
            }
        }

        public List<CustomerDetailDto> GetCustomerDetails()
        {
            using (var context = new RentACarContext())
            {
                var result = from c in context.Customers
                             select new CustomerDetailDto
                             {
                                 Id = c.Id,
                                 Email = c.Users.FirstOrDefault() != null ? c.Users.FirstOrDefault().Email : null,
                                 PhoneNumber = c.PhoneNumber,
                                 IdentityNumber = (c is IndividualCustomer) ? ((IndividualCustomer)c).IdentityNumber : null,
                                 UserId = c.Users.FirstOrDefault() != null ? (int?)c.Users.FirstOrDefault().Id : null,
                                 CreatedDate = c.CreatedDate,
                                 FirstName = (c is IndividualCustomer) ? ((IndividualCustomer)c).FirstName : null,
                                 LastName = (c is IndividualCustomer) ? ((IndividualCustomer)c).LastName : null,
                                 CompanyName = (c is CorporateCustomer) ? ((CorporateCustomer)c).CompanyName : null,
                                 TaxNumber = (c is CorporateCustomer) ? ((CorporateCustomer)c).TaxNumber : null
                             };
                return result.ToList();
            }
        }

        public async Task<List<CustomerDetailDto>> GetCustomerDetailsAsync()
        {
            using (var context = new RentACarContext())
            {
                var result = from c in context.Customers
                             select new CustomerDetailDto
                             {
                                 Id = c.Id,
                                 Email = c.Users.FirstOrDefault() != null ? c.Users.FirstOrDefault().Email : null,
                                 PhoneNumber = c.PhoneNumber,
                                 IdentityNumber = (c is IndividualCustomer) ? ((IndividualCustomer)c).IdentityNumber : null,
                                 UserId = c.Users.FirstOrDefault() != null ? (int?)c.Users.FirstOrDefault().Id : null,
                                 CreatedDate = c.CreatedDate,
                                 FirstName = (c is IndividualCustomer) ? ((IndividualCustomer)c).FirstName : null,
                                 LastName = (c is IndividualCustomer) ? ((IndividualCustomer)c).LastName : null,
                                 CompanyName = (c is CorporateCustomer) ? ((CorporateCustomer)c).CompanyName : null,
                                 TaxNumber = (c is CorporateCustomer) ? ((CorporateCustomer)c).TaxNumber : null
                             };
                return await result.ToListAsync();
            }
        }
    }
}
