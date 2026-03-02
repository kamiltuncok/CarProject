using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfRentalDal : EfEntityRepositoryBase<Rental, RentACarContext>, IRentalDal
    {
        // Helper: builds the base LINQ join query for rental details
        private IQueryable<RentalDetailDto> BuildRentalDetailQuery(RentACarContext context)
        {
            return from r in context.Rentals
                   join c in context.Cars on r.CarId equals c.Id
                   join b in context.Brands on c.BrandId equals b.BrandId
                   join co in context.Colors on c.ColorId equals co.ColorId
                   join s in context.Segments on c.SegmentId equals s.SegmentId
                   join f in context.Fuels on c.FuelId equals f.FuelId
                   join g in context.Gears on c.GearId equals g.GearId
                   join startLoc in context.Locations on r.StartLocationId equals startLoc.Id
                   join startCity in context.LocationCities on startLoc.LocationCityId equals startCity.Id
                   join endLoc in context.Locations on r.EndLocationId equals endLoc.Id
                   join endCity in context.LocationCities on endLoc.LocationCityId equals endCity.Id
                   join cust in context.Customers on r.CustomerId equals cust.Id
                   select new RentalDetailDto
                   {
                       Id = r.Id,
                       CarId = c.Id,
                       CustomerId = r.CustomerId,
                       BrandName = b.BrandName,
                       ColorName = co.ColorName,
                       SegmentName = s.SegmentName,
                       FuelName = f.FuelName,
                       GearName = g.GearName,
                       ModelYear = c.ModelYear,
                       DailyPrice = c.DailyPrice,
                       Description = c.Description,
                       PlateNumber = c.PlateNumber,
                       StartLocationName = startLoc.LocationName,
                       StartLocationCity = startCity.Name,    // resolved from LocationCities table
                       EndLocationName = endLoc.LocationName,
                       EndLocationCity = endCity.Name,        // resolved from LocationCities table
                       StartDate = r.StartDate,
                       EndDate = r.EndDate,
                       RentedDailyPrice = r.RentedDailyPrice,
                       TotalPrice = r.TotalPrice,
                       DepositAmount = r.DepositAmount,
                       DepositDeductedAmount = r.DepositDeductedAmount,
                       DepositRefundedDate = r.DepositRefundedDate,
                       DepositStatus = r.DepositStatus,
                       Status = r.Status,
                       CustomerEmail = cust.Users.FirstOrDefault() != null ? cust.Users.FirstOrDefault().Email : null,
                       CustomerPhone = cust.PhoneNumber
                   };
        }

        public List<RentalDetailDto> GetRentalDetailsByUserId(int userId)
        {
            using (var context = new RentACarContext())
            {
                // Find customerId(s) linked to this userId
                var customerIds = context.Customers
                    .Where(c => c.Users.Any(u => u.Id == userId))
                    .Select(c => c.Id)
                    .ToList();

                return BuildRentalDetailQuery(context)
                    .Where(r => customerIds.Contains(r.CustomerId))
                    .ToList();
            }
        }

        public List<RentalDetailDto> GetRentalDetailsByLocationName(string locationName)
        {
            using (var context = new RentACarContext())
            {
                return BuildRentalDetailQuery(context)
                    .Where(r => r.StartLocationName == locationName || r.EndLocationName == locationName)
                    .ToList();
            }
        }

        public List<RentalDetailDto> GetRentalsByEmail(string email)
        {
            using (var context = new RentACarContext())
            {
                return BuildRentalDetailQuery(context)
                    .Where(r => r.CustomerEmail == email)
                    .ToList();
            }
        }

        public List<RentalDetailDto> GetRentalsByName(string name)
        {
            using (var context = new RentACarContext())
            {
                var search = name.ToLower().Trim();

                // 1. Search Customers (Base) by Phone
                var baseMatchingIds = context.Customers
                    .Where(c => c.PhoneNumber.Contains(search))
                    .Select(c => c.Id)
                    .ToList();

                // 2. Search IndividualCustomers directly (Guest or Registered)
                var individualMatchingIds = context.IndividualCustomers
                    .Where(ic => (ic.FirstName + " " + ic.LastName).ToLower().Contains(search) || ic.IdentityNumber.Contains(search))
                    .Select(ic => ic.Id)
                    .ToList();

                // 3. Search CorporateCustomers directly
                var corporateMatchingIds = context.CorporateCustomers
                    .Where(cc => cc.CompanyName.ToLower().Contains(search))
                    .Select(cc => cc.Id)
                    .ToList();

                // Combine all unique customer IDs
                var allCustomerIds = baseMatchingIds
                    .Union(individualMatchingIds)
                    .Union(corporateMatchingIds)
                    .Distinct()
                    .ToList();

                return BuildRentalDetailQuery(context)
                    .Where(r => allCustomerIds.Contains(r.CustomerId))
                    .ToList();
            }
        }

        public List<RentalDetailDto> GetRentalsByDateRange(DateTime startDate, DateTime endDate)
        {
            using (var context = new RentACarContext())
            {
                return BuildRentalDetailQuery(context)
                    .Where(r => r.StartDate >= startDate.Date && r.StartDate < endDate.Date.AddDays(1))
                    .ToList();
            }
        }

        public void AddRange(List<Rental> rentals)
        {
            using (var context = new RentACarContext())
            {
                context.Rentals.AddRange(rentals);
                context.SaveChanges();
            }
        }
    }
}