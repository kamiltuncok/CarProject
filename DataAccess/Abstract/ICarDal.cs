using Core.DataAccess;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

        Task<List<CarDetailDto>> GetCarDetailsByBrandIdAsync(int brandId);
        Task<List<CarDetailDto>> GetCarDetailsByColorIdAsync(int colorId);
        Task<CarDetailDto> GetCarDetailsByIdAsync(int id);
        Task<List<CarDetailDto>> GetCarDetailsAsync(Expression<Func<CarDetailDto, bool>> filter = null);
        
        Task<List<CarDetailDto>> GetAvailableCarsAsync(CarAvailabilityFilterDto filter);

        Task<decimal> GetLowestPriceBySegmentIdAsync(int segmentId, bool isRented);
    }
}
