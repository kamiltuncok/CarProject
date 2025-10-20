using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCarDal : EfEntityRepositoryBase<Car, RentACarContext>, ICarDal
    {

        public List<CarDetailDto> GetCarDetailsByColorId(int colorId)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = from ca in context.Cars
                             join co in context.Colors
                             on ca.ColorId equals co.ColorId
                             join br in context.Brands
                             on ca.BrandId equals br.BrandId
                             join fu in context.Fuels
                             on ca.FuelId equals fu.FuelId
                             join ge in context.Gears
                             on ca.GearId equals ge.GearId
                             join lo in context.Locations
                             on ca.LocationId equals lo.LocationId
                             where ca.ColorId == colorId

                             select new CarDetailDto
                             {
                                 CarId = ca.Id,
                                 BrandId = ca.BrandId,
                                 ColorId = ca.ColorId,
                                 BrandName = br.BrandName,
                                 ColorName = co.ColorName,
                                 DailyPrice = ca.DailyPrice,
                                 Description = ca.Description,
                                 ModelYear = ca.ModelYear,
                                 LocationName = lo.LocationName,
                                 FuelName = fu.FuelName,
                                 GearName = ge.GearName,
                                 Deposit = ca.Deposit
                             };
                return result.ToList();

            }
        }



        public List<CarDetailDto> GetCarDetailsByBrandId(int brandId)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = from ca in context.Cars
                             join co in context.Colors
                             on ca.ColorId equals co.ColorId
                             join br in context.Brands
                             on ca.BrandId equals br.BrandId
                             join fu in context.Fuels
                             on ca.FuelId equals fu.FuelId
                             join ge in context.Gears
                             on ca.GearId equals ge.GearId
                             join lo in context.Locations
                             on ca.LocationId equals lo.LocationId
                             where ca.BrandId == brandId

                             select new CarDetailDto
                             {
                                 CarId = ca.Id,
                                 BrandId = ca.BrandId,
                                 ColorId = ca.ColorId,
                                 BrandName = br.BrandName,
                                 ColorName = co.ColorName,
                                 DailyPrice = ca.DailyPrice,
                                 Description = ca.Description,
                                 ModelYear = ca.ModelYear,
                                 LocationName = lo.LocationName,
                                 FuelName = fu.FuelName,
                                 GearName = ge.GearName,
                                 Deposit = ca.Deposit
                             };
                return result.ToList();

            }
        }
        public List<CarDetailDto> GetCarDetailsByCarId(int carId)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = from ca in context.Cars
                             join co in context.Colors
                             on ca.ColorId equals co.ColorId
                             join br in context.Brands
                             on ca.BrandId equals br.BrandId
                             join fu in context.Fuels
                             on ca.FuelId equals fu.FuelId
                             join ge in context.Gears
                             on ca.GearId equals ge.GearId
                             join se in context.Segments
                             on ca.SegmentId equals se.SegmentId
                             join lo in context.Locations
                             on ca.LocationId equals lo.LocationId
                             where ca.Id == carId

                             select new CarDetailDto
                             {
                                 CarId = ca.Id,
                                 BrandId = ca.BrandId,
                                 ColorId = ca.ColorId,
                                 BrandName = br.BrandName,
                                 ColorName = co.ColorName,
                                 DailyPrice = ca.DailyPrice,
                                 Description = ca.Description,
                                 ModelYear = ca.ModelYear,
                                 LocationName = lo.LocationName,
                                 IsRented = ca.IsRented,
                                 FuelName = fu.FuelName,
                                 GearName = ge.GearName,
                                 SegmentName = se.SegmentName,
                                 Deposit = ca.Deposit
                             };
                return result.ToList();

            }
        }
        public CarDetailDto GetCarDetailsById(int id)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = from ca in context.Cars
                             join co in context.Colors
                             on ca.ColorId equals co.ColorId
                             join br in context.Brands
                             on ca.BrandId equals br.BrandId
                             join fu in context.Fuels
                             on ca.FuelId equals fu.FuelId
                             join ge in context.Gears
                             on ca.GearId equals ge.GearId
                             join se in context.Segments
                             on ca.SegmentId equals se.SegmentId
                             join lo in context.Locations
                             on ca.LocationId equals lo.LocationId
                             where ca.Id == id

                             select new CarDetailDto
                             {
                                 CarId = ca.Id,
                                 BrandId = ca.BrandId,
                                 ColorId = ca.ColorId,
                                 BrandName = br.BrandName,
                                 ColorName = co.ColorName,
                                 DailyPrice = ca.DailyPrice,
                                 Description = ca.Description,
                                 ModelYear = ca.ModelYear,
                                 LocationName = lo.LocationName,
                                 FuelName = fu.FuelName,
                                 GearName = ge.GearName,
                                 SegmentName = se.SegmentName,
                                 Deposit = ca.Deposit
                             };
                return result.First();
            }
        }
        public List<CarDetailDto> GetCarDetails(Expression<Func<CarDetailDto, bool>> filter = null)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = from ca in context.Cars
                             join co in context.Colors
                             on ca.ColorId equals co.ColorId
                             join br in context.Brands
                             on ca.BrandId equals br.BrandId
                             join fu in context.Fuels
                             on ca.FuelId equals fu.FuelId
                             join ge in context.Gears
                             on ca.GearId equals ge.GearId
                             join se in context.Segments
                             on ca.SegmentId equals se.SegmentId
                             join lo in context.Locations
                             on ca.LocationId equals lo.LocationId

                             select new CarDetailDto
                             {
                                 CarId = ca.Id,
                                 BrandId = ca.BrandId,
                                 ColorId = ca.ColorId,
                                 BrandName = br.BrandName,
                                 ColorName = co.ColorName,
                                 DailyPrice = ca.DailyPrice,
                                 Description = ca.Description,
                                 ModelYear = ca.ModelYear,
                                 LocationName = lo.LocationName,
                                 FuelName = fu.FuelName,
                                 GearName = ge.GearName,
                                 SegmentName = se.SegmentName,
                                 Deposit = ca.Deposit
                             };
                return filter == null
             ? result.ToList()
             : result.Where(filter).ToList();
            };
        }

        public List<CarDetailDto> GetNotRentedCarsByLocationId(int locationId, bool IsRented)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = from ca in context.Cars
                             join co in context.Colors
                             on ca.ColorId equals co.ColorId
                             join br in context.Brands
                             on ca.BrandId equals br.BrandId
                             join fu in context.Fuels
                             on ca.FuelId equals fu.FuelId
                             join ge in context.Gears
                             on ca.GearId equals ge.GearId
                             join se in context.Segments
                             on ca.SegmentId equals se.SegmentId
                             join lo in context.Locations
                             on ca.LocationId equals lo.LocationId
                             where ca.LocationId == locationId && ca.IsRented == IsRented

                             select new CarDetailDto
                             {
                                 CarId = ca.Id,
                                 BrandId = ca.BrandId,
                                 ColorId = ca.ColorId,
                                 BrandName = br.BrandName,
                                 ColorName = co.ColorName,
                                 DailyPrice = ca.DailyPrice,
                                 Description = ca.Description,
                                 ModelYear = ca.ModelYear,
                                 LocationName = lo.LocationName,
                                 FuelName = fu.FuelName,
                                 GearName = ge.GearName,
                                 SegmentName = se.SegmentName,
                                 Deposit = ca.Deposit

                             };
                return result.ToList();

            }
        }

        public List<CarDetailDto> GetCarsNotRentedByLocationName(string locationName, bool IsRented)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = from ca in context.Cars
                             join co in context.Colors
                             on ca.ColorId equals co.ColorId
                             join br in context.Brands
                             on ca.BrandId equals br.BrandId
                             join fu in context.Fuels
                             on ca.FuelId equals fu.FuelId
                             join ge in context.Gears
                             on ca.GearId equals ge.GearId
                             join lo in context.Locations
                             on ca.LocationId equals lo.LocationId
                             where lo.LocationName == locationName && ca.IsRented == IsRented

                             select new CarDetailDto
                             {
                                 CarId = ca.Id,
                                 BrandId = ca.BrandId,
                                 ColorId = ca.ColorId,
                                 BrandName = br.BrandName,
                                 ColorName = co.ColorName,
                                 DailyPrice = ca.DailyPrice,
                                 Description = ca.Description,
                                 ModelYear = ca.ModelYear,
                                 LocationName = lo.LocationName,
                                 FuelName = fu.FuelName,
                                 GearName = ge.GearName,
                                 Deposit = ca.Deposit
                             };
                return result.ToList();

            }
        }


        public List<CarDetailDto> GetCarsByFilters(List<int> fuelIds, List<int> gearIds, List<int> segmentIds, bool isRented, string locationName)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var query = from ca in context.Cars
                            join co in context.Colors
                            on ca.ColorId equals co.ColorId
                            join br in context.Brands
                            on ca.BrandId equals br.BrandId
                            join fu in context.Fuels
                            on ca.FuelId equals fu.FuelId
                            join ge in context.Gears
                            on ca.GearId equals ge.GearId
                            join se in context.Segments
                            on ca.SegmentId equals se.SegmentId
                            join lo in context.Locations
                            on ca.LocationId equals lo.LocationId
                            where ca.IsRented == isRented &&
                                  lo.LocationName == locationName

                            select new CarDetailDto
                            {
                                CarId = ca.Id,
                                BrandId = ca.BrandId,
                                ColorId = ca.ColorId,
                                BrandName = br.BrandName,
                                ColorName = co.ColorName,
                                DailyPrice = ca.DailyPrice,
                                Description = ca.Description,
                                ModelYear = ca.ModelYear,
                                LocationName = lo.LocationName,
                                FuelName = fu.FuelName,
                                GearName = ge.GearName,
                                SegmentName = se.SegmentName,
                                Deposit = ca.Deposit,
                                IsRented = ca.IsRented,
                                FuelId = ca.FuelId,
                                GearId = ca.GearId,
                                SegmentId = ca.SegmentId
                            };

                // Fuel filtreleri
                if (fuelIds != null && fuelIds.Any())
                {
                    query = query.Where(c => fuelIds.Contains(c.FuelId));
                }

                // Gear filtreleri
                if (gearIds != null && gearIds.Any())
                {
                    query = query.Where(c => gearIds.Contains(c.GearId));
                }

                // Segment filtreleri
                if (segmentIds != null && segmentIds.Any())
                {
                    query = query.Where(c => segmentIds.Contains(c.SegmentId));
                }

                return query.ToList();
            }
        
    }

        public decimal GetLowestPriceBySegmentId(int segmentId, bool isRented)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var query = from car in context.Cars
                            where car.SegmentId == segmentId && car.IsRented == isRented
                            orderby car.DailyPrice
                            select car.DailyPrice;

                var lowestPrice = query.FirstOrDefault();
                return lowestPrice;
            }
        }

    }
}
