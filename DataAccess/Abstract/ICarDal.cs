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
        CarDetailDto GetCarDetailsById(int id);
        List<CarDetailDto> GetCarDetails(Expression<Func<CarDetailDto, bool>> filter = null);
        
        // Single unified availability endpoint replacing GetCarsNotRented... and GetCarsByFilters
        List<CarDetailDto> GetAvailableCars(CarAvailabilityFilterDto filter);

        decimal GetLowestPriceBySegmentId(int segmentId, bool isRented);
    }
}
