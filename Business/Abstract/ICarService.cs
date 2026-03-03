using Core.Utilities;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
   public interface ICarService
    {
        IDataResult<List<Car>> GetAll();

        IDataResult<List<Car>> GetById(int id);
        IDataResult<List<Car>> GetCarsByBrandId(int id);
        IDataResult<List<Car>> GetCarsByColorId(int id);
        IDataResult<List<CarDetailDto>> GetCarDetailDtos();
        IResult Add(Car car);
        IResult Delete(Car car);
        IResult Update(Car car);
        IResult AddTransactionalTest(Car car);
        IDataResult<List<CarDetailDto>> GetCarDetails();
        IDataResult<List<CarDetailDto>> GetCarDetailsByBrandId(int brandId);
        IDataResult<List<CarDetailDto>> GetCarDetailsByColorId(int colorId);
        IDataResult<CarDetailDto> GetCarDetailsById(int id);
        
        // Async unified availability endpoint
        Task<IDataResult<List<CarDetailDto>>> GetAvailableCarsAsync(CarAvailabilityFilterDto filter);

        Task<IResult> CarRentedAsync(int carId);

        Task<IDataResult<decimal>> UpdatePriceByActionAsync(int carId, string action);

        Task<IDataResult<decimal>> GetLowestPriceBySegmentIdAsync(int segmentId, bool isRented);
        
        // Async standard endpoints with DTO decoupling
        Task<IDataResult<List<Car>>> GetAllAsync();
        Task<IDataResult<List<CarDetailDto>>> GetCarDetailDtosAsync();
        Task<IDataResult<List<CarDetailDto>>> GetCarDetailsByBrandIdAsync(int brandId);
        Task<IDataResult<List<CarDetailDto>>> GetCarDetailsByColorIdAsync(int colorId);
        Task<IDataResult<CarDetailDto>> GetCarDetailsByIdAsync(int id);
        Task<IDataResult<List<Car>>> GetByIdAsync(int id);
        Task<IResult> AddAsync(CarCreateDto carDto);
        Task<IResult> UpdateAsync(CarUpdateDto carDto);
        Task<IResult> DeleteAsync(int id);
    }

}

