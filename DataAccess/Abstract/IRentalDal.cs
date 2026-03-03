using Core.DataAccess;
using Entities.Concrete;
using Entities.DTOs;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        Task<List<RentalDetailDto>> GetRentalDetailsByUserIdAsync(int userId);
        Task<List<RentalDetailDto>> GetRentalDetailsByLocationNameAsync(string locationName);
        Task<List<RentalDetailDto>> GetRentalsByEmailAsync(string email);
        Task<List<RentalDetailDto>> GetRentalsByNameAsync(string name);
        Task<List<RentalDetailDto>> GetRentalsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task AddRangeAsync(List<Rental> rentals);
    }
}
