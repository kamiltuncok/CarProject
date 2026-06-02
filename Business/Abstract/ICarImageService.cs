using Core.Utilities;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
   public interface ICarImageService
    {
        IDataResult<List<CarImage>> GetAll();
        Task<IDataResult<List<CarImage>>> GetAllAsync();
        IDataResult<CarImage> GetById(int id);
        Task<IDataResult<CarImage>> GetByIdAsync(int id);
        IResult Add(IFormFile file, CarImage carImage);
        Task<IResult> AddAsync(IFormFile file, CarImage carImage);
        IResult Update(CarImage carImage, IFormFile file);
        Task<IResult> UpdateAsync(CarImage carImage, IFormFile file);
        IResult Delete(CarImage carImage);
        Task<IResult> DeleteAsync(CarImage carImage);
        IDataResult<List<CarImage>> GetByCarId(int carId);
        Task<IDataResult<List<CarImage>>> GetByCarIdAsync(int carId);
    }
}
