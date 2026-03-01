using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                                 Email = c.Email,
                                 PhoneNumber = c.PhoneNumber,
                                 IdentityNumber = c.IdentityNumber,
                                 UserId = c.UserId,
                                 CreatedDate = c.CreatedDate,
                                 FirstName = (c is IndividualCustomer) ? ((IndividualCustomer)c).FirstName : null,
                                 LastName = (c is IndividualCustomer) ? ((IndividualCustomer)c).LastName : null,
                                 CompanyName = (c is CorporateCustomer) ? ((CorporateCustomer)c).CompanyName : null,
                                 TaxNumber = (c is CorporateCustomer) ? ((CorporateCustomer)c).TaxNumber : null
                             };
                return result.FirstOrDefault();
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
                                 Email = c.Email,
                                 PhoneNumber = c.PhoneNumber,
                                 IdentityNumber = c.IdentityNumber,
                                 UserId = c.UserId,
                                 CreatedDate = c.CreatedDate,
                                 FirstName = (c is IndividualCustomer) ? ((IndividualCustomer)c).FirstName : null,
                                 LastName = (c is IndividualCustomer) ? ((IndividualCustomer)c).LastName : null,
                                 CompanyName = (c is CorporateCustomer) ? ((CorporateCustomer)c).CompanyName : null,
                                 TaxNumber = (c is CorporateCustomer) ? ((CorporateCustomer)c).TaxNumber : null
                             };
                return result.ToList();
            }
        }
    }
}
