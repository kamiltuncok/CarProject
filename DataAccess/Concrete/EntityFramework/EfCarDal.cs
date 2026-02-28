using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCarDal : EfEntityRepositoryBase<Car, RentACarContext>, ICarDal
    {
        // Helper to build a base car detail query
        private IQueryable<CarDetailDto> BuildCarDetailQuery(RentACarContext context)
        {
            return from ca in context.Cars
                   join co in context.Colors on ca.ColorId equals co.ColorId
                   join br in context.Brands on ca.BrandId equals br.BrandId
                   join fu in context.Fuels on ca.FuelId equals fu.FuelId
                   join ge in context.Gears on ca.GearId equals ge.GearId
                   join se in context.Segments on ca.SegmentId equals se.SegmentId
                   join lo in context.Locations on ca.CurrentLocationId equals lo.Id
                   select new CarDetailDto
                   {
                       Id = ca.Id,
                       BrandId = ca.BrandId,
                       ColorId = ca.ColorId,
                       FuelId = ca.FuelId,
                       GearId = ca.GearId,
                       SegmentId = ca.SegmentId,
                       BrandName = br.BrandName,
                       ColorName = co.ColorName,
                       FuelName = fu.FuelName,
                       GearName = ge.GearName,
                       SegmentName = se.SegmentName,
                       LocationName = lo.LocationName,
                       LocationCity = lo.LocationCity,
                       DailyPrice = ca.DailyPrice,
                       Deposit = ca.Deposit,
                       Description = ca.Description,
                       ModelYear = ca.ModelYear,
                       Status = ca.Status,
                       PlateNumber = ca.PlateNumber,
                       KM = ca.KM
                   };
        }

        public List<CarDetailDto> GetCarDetailsByColorId(int colorId)
        {
            using (var context = new RentACarContext())
            {
                return BuildCarDetailQuery(context)
                    .Where(c => c.ColorId == colorId)
                    .ToList();
            }
        }

        public List<CarDetailDto> GetCarDetailsByBrandId(int brandId)
        {
            using (var context = new RentACarContext())
            {
                return BuildCarDetailQuery(context)
                    .Where(c => c.BrandId == brandId)
                    .ToList();
            }
        }

        public CarDetailDto GetCarDetailsById(int id)
        {
            using (var context = new RentACarContext())
            {
                return BuildCarDetailQuery(context)
                    .FirstOrDefault(c => c.Id == id);
            }
        }

        public List<CarDetailDto> GetCarDetails(Expression<Func<CarDetailDto, bool>> filter = null)
        {
            using (var context = new RentACarContext())
            {
                var query = BuildCarDetailQuery(context);
                return filter == null ? query.ToList() : query.Where(filter).ToList();
            }
        }

        public List<CarDetailDto> GetAvailableCars(CarAvailabilityFilterDto filter)
        {
            using (var context = new RentACarContext())
            {
                // 1. Basic Filters: Must be at start location and cannot be in maintenance
                var query = context.Cars.Where(c =>
                    c.CurrentLocationId == filter.StartLocationId &&
                    c.Status != CarStatus.Maintenance);

                // 2. Strict Overlap Rule checking Active/Pending rentals
                query = query.Where(c => !context.Rentals.Any(r =>
                    r.CarId == c.Id &&
                    (r.Status == RentalStatus.Active || r.Status == RentalStatus.Pending) &&
                    r.StartDate < filter.EndDate &&
                    r.EndDate > filter.StartDate
                ));

                // 3. Optional User Filters
                if (filter.FuelIds != null && filter.FuelIds.Any())
                    query = query.Where(c => filter.FuelIds.Contains(c.FuelId));

                if (filter.GearIds != null && filter.GearIds.Any())
                    query = query.Where(c => filter.GearIds.Contains(c.GearId));

                if (filter.SegmentIds != null && filter.SegmentIds.Any())
                    query = query.Where(c => filter.SegmentIds.Contains(c.SegmentId));

                // 4. Output Projection using existing BuildCarDetailQuery logic mapped over the filtered query
                // Instead of calling BuildCarDetailQuery() which joins everything indiscriminately, 
                // we join our perfectly filtered cars with the other tables.
                
                var detailedQuery = from ca in query
                                   join co in context.Colors on ca.ColorId equals co.ColorId
                                   join br in context.Brands on ca.BrandId equals br.BrandId
                                   join fu in context.Fuels on ca.FuelId equals fu.FuelId
                                   join ge in context.Gears on ca.GearId equals ge.GearId
                                   join se in context.Segments on ca.SegmentId equals se.SegmentId
                                   join lo in context.Locations on ca.CurrentLocationId equals lo.Id
                                   select new CarDetailDto
                                   {
                                       Id = ca.Id,
                                       BrandId = ca.BrandId,
                                       ColorId = ca.ColorId,
                                       FuelId = ca.FuelId,
                                       GearId = ca.GearId,
                                       SegmentId = ca.SegmentId,
                                       BrandName = br.BrandName,
                                       ColorName = co.ColorName,
                                       FuelName = fu.FuelName,
                                       GearName = ge.GearName,
                                       SegmentName = se.SegmentName,
                                       LocationName = lo.LocationName,
                                       LocationCity = lo.LocationCity,
                                       DailyPrice = ca.DailyPrice,
                                       Deposit = ca.Deposit,
                                       Description = ca.Description,
                                       ModelYear = ca.ModelYear,
                                       Status = ca.Status,
                                       PlateNumber = ca.PlateNumber,
                                       KM = ca.KM
                                   };

                return detailedQuery.ToList();
            }
        }

        public decimal GetLowestPriceBySegmentId(int segmentId, bool isRented)
        {
            using (var context = new RentACarContext())
            {
                var statusFilter = isRented ? CarStatus.Rented : CarStatus.Available;
                return context.Cars
                    .Where(c => c.SegmentId == segmentId && c.Status == statusFilter)
                    .OrderBy(c => c.DailyPrice)
                    .Select(c => c.DailyPrice)
                    .FirstOrDefault();
            }
        }
    }
}
