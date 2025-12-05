using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
   public interface IRentalDal:IEntityRepository<Rental>
    {
        List<RentalDetailDto> GetRentalDetailsByUserId(int userId, CustomerType customerType);
        List<RentalDetailDto> GetRentalDetailsByLocationName(string locationName);
        void AddRange(List<Rental> rentals);
    }
}
