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
                                 FuelName=fu.FuelName,
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
                                 FuelName=fu.FuelName,
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
                                 IsRented =ca.IsRented,
                                 FuelName=fu.FuelName,
                                 GearName = ge.GearName,
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
                                 FuelName=fu.FuelName,
                                 GearName = ge.GearName,
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
                                 LocationName=lo.LocationName,
                                 FuelName=fu.FuelName,
                                 GearName = ge.GearName,
                                 Deposit = ca.Deposit
                             };
                return filter == null
             ? result.ToList()
             : result.Where(filter).ToList();
            };
        }

        public List<CarDetailDto> GetCarDetailsByLocationId(int locationId)
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
                             where ca.LocationId == locationId

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
                                 FuelName= fu.FuelName,
                                 GearName = ge.GearName,
                                 Deposit = ca.Deposit
                             };
                return result.ToList();

            }
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
                             join lo in context.Locations
                             on ca.LocationId equals lo.LocationId
                             where ca.LocationId == locationId && ca.IsRented ==IsRented

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
                                 FuelName= fu.FuelName,
                                 GearName = ge.GearName,
                                 Deposit =ca.Deposit
                             };
                return result.ToList();

            }
        }
    }
}
