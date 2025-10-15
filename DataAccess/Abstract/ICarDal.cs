using Core.DataAccess;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
   public interface ICarDal:IEntityRepository<Car>
    {
        List<CarDetailDto> GetCarDetailsByBrandId(int brandId);
        List<CarDetailDto> GetCarDetailsByColorId(int colorId);
        List<CarDetailDto> GetCarDetailsByLocationId(int locationId);
        List<CarDetailDto> GetCarDetailsByGearId(int gearId, bool IsRented);
        List<CarDetailDto> GetCarDetailsByFuelId(int fuelId, bool IsRented);
        List<CarDetailDto> GetNotRentedCarsByLocationId(int locationId,bool IsRented);
        List<CarDetailDto> GetCarsNotRentedByLocationName(string locationName, bool IsRented);
        List<CarDetailDto> GetCarDetailsByCarId(int carId);
        CarDetailDto GetCarDetailsById(int id);
        List<CarDetailDto> GetCarDetails(Expression<Func<CarDetailDto, bool>> filter = null);

        List<CarDetailDto> GetCarsByFuelAndLocation(int fuelId, bool isRented, string locationName);
        List<CarDetailDto> GetCarsByGearAndLocation(int gearId, bool isRented, string locationName);
        List<CarDetailDto> GetCarsByGearAndFuelFilters(List<int> fuelIds, List<int> gearIds, bool isRented, string locationName);
    }
}
