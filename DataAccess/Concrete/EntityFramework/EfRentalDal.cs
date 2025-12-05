using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Entities.Enums;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfRentalDal : EfEntityRepositoryBase<Rental, RentACarContext>, IRentalDal
    {
        public List<RentalDetailDto> GetRentalDetailsByUserId(int userId, CustomerType customerType)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = from r in context.Rentals
                             join c in context.Cars on r.CarId equals c.Id
                             join b in context.Brands on c.BrandId equals b.BrandId
                             join co in context.Colors on c.ColorId equals co.ColorId
                             join s in context.Segments on c.SegmentId equals s.SegmentId
                             join f in context.Fuels on c.FuelId equals f.FuelId
                             join g in context.Gears on c.GearId equals g.GearId
                             join l in context.Locations on c.LocationId equals l.LocationId
                             where r.UserId == userId && r.CustomerType == customerType
                             select new RentalDetailDto
                             {
                                 RentalId = r.RentalId,
                                 CarId = c.Id,
                                 UserId = r.UserId,
                                 BrandName = b.BrandName,
                                 ColorName = co.ColorName,
                                 SegmentName = s.SegmentName,
                                 FuelName = f.FuelName,
                                 GearName = g.GearName,
                                 LocationName = l.LocationName,
                                 ModelYear = c.ModelYear,
                                 DailyPrice = c.DailyPrice,
                                 Description = c.Description,
                                 RentDate = r.RentDate,
                                 ReturnDate = r.ReturnDate,
                                 StartLocation = r.StartLocation,
                                 EndLocation = r.EndLocation,
                                 IsReturned = r.isReturned,
                                 Deposit = c.Deposit,
                                 CustomerType = r.CustomerType
                             };
                return result.ToList();
            }
        }

        public List<RentalDetailDto> GetRentalDetailsByLocationName(string locationName)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = from r in context.Rentals
                             join c in context.Cars on r.CarId equals c.Id
                             join b in context.Brands on c.BrandId equals b.BrandId
                             join co in context.Colors on c.ColorId equals co.ColorId
                             join s in context.Segments on c.SegmentId equals s.SegmentId
                             join f in context.Fuels on c.FuelId equals f.FuelId
                             join g in context.Gears on c.GearId equals g.GearId
                             join l in context.Locations on c.LocationId equals l.LocationId
                             where r.StartLocation == locationName 
                             select new RentalDetailDto
                             {
                                 RentalId = r.RentalId,
                                 CarId = c.Id,
                                 UserId = r.UserId,
                                 BrandName = b.BrandName,
                                 CustomerId= c.Id,
                                 ColorName = co.ColorName,
                                 SegmentName = s.SegmentName,
                                 FuelName = f.FuelName,
                                 GearName = g.GearName,
                                 LocationName = l.LocationName,
                                 ModelYear = c.ModelYear,
                                 DailyPrice = c.DailyPrice,
                                 Description = c.Description,
                                 RentDate = r.RentDate,
                                 ReturnDate = r.ReturnDate,
                                 StartLocation = r.StartLocation,
                                 EndLocation = r.EndLocation,
                                 IsReturned = r.isReturned,
                                 Deposit = c.Deposit,
                                 CustomerType = r.CustomerType
                             };
                return result.ToList();
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