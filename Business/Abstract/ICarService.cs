using Core.Utilities;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

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
        IDataResult<List<CarDetailDto>> GetCarsNotRentedByLocationName(string locationName, bool IsRented);
        IDataResult<CarDetailDto> GetCarDetailsById(int id);
        void CarRented(int carId);

        IDataResult<int> UpdatePriceByAction(int carId, string action);

        IDataResult<List<CarDetailDto>> GetCarsByFilters(List<int> fuelIds, List<int> gearIds, List<int> segmentIds, bool isRented, string locationName);

        IDataResult<decimal> GetLowestPriceBySegmentId(int segmentId, bool isRented);
    }

}

