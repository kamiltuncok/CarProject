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
        List<RentalDetailDto> GetRentalDetailsByUserId(int userId);
        List<RentalDetailDto> GetRentalDetailsByLocationName(string locationName);
        List<RentalDetailDto> GetRentalsByEmail(string email);
        List<RentalDetailDto> GetRentalsByName(string name);
        List<RentalDetailDto> GetRentalsByDateRange(DateTime startDate, DateTime endDate);
        void AddRange(List<Rental> rentals);
    }
}
