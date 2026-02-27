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
                       CarId = ca.Id,
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
                    .FirstOrDefault(c => c.CarId == id);
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

        public List<CarDetailDto> GetCarsNotRentedByLocationName(string locationName, bool IsRented)
        {
            using (var context = new RentACarContext())
            {
                // IsRented=false => CarStatus.Available
                var statusFilter = IsRented ? CarStatus.Rented : CarStatus.Available;
                return BuildCarDetailQuery(context)
                    .Where(c => c.LocationName == locationName && c.Status == statusFilter)
                    .ToList();
            }
        }

        public List<CarDetailDto> GetCarsByFilters(List<int> fuelIds, List<int> gearIds, List<int> segmentIds, bool isRented, string locationName)
        {
            using (var context = new RentACarContext())
            {
                var statusFilter = isRented ? CarStatus.Rented : CarStatus.Available;
                var query = BuildCarDetailQuery(context)
                    .Where(c => c.Status == statusFilter && c.LocationName == locationName);

                if (fuelIds != null && fuelIds.Any())
                    query = query.Where(c => fuelIds.Contains(c.FuelId));

                if (gearIds != null && gearIds.Any())
                    query = query.Where(c => gearIds.Contains(c.GearId));

                if (segmentIds != null && segmentIds.Any())
                    query = query.Where(c => segmentIds.Contains(c.SegmentId));

                return query.ToList();
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
