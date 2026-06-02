using Business.Abstract;
using Core.Utilities.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        private readonly ICarImageDal _carImageDal;

        public CarImageManager(ICarImageDal carImageDal)
        {
            _carImageDal = carImageDal;
        }

        public IResult Add(IFormFile file, CarImage carImage)
        {
            carImage.ImagePath = FileHelper.Add(file);
            _carImageDal.Add(carImage);
            return new SuccessResult("Resim eklendi");
        }

        public IResult Delete(CarImage carImage)
        {
            new FileHelper().Delete(carImage.ImagePath);
            _carImageDal.Delete(carImage);
            return new SuccessResult("Resim silindi");
        }

        public IDataResult<List<CarImage>> GetAll()
        {
            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll());
        }

        public IDataResult<CarImage> GetById(int id)
        {
            return new SuccessDataResult<CarImage>(_carImageDal.Get(c => c.Id == id));
        }

        public IDataResult<List<CarImage>> GetByCarId(int carId)
        {
            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll(c => c.CarId == carId));
        }

        public IResult Update(CarImage carImage, IFormFile file)
        {
            var existingImage = _carImageDal.Get(p => p.Id == carImage.Id);
            carImage.ImagePath = FileHelper.Update(existingImage?.ImagePath, file);
            _carImageDal.Update(carImage);
            return new SuccessResult("Resim güncellendi");
        }

        public async Task<IResult> AddAsync(IFormFile file, CarImage carImage)
        {
            carImage.ImagePath = FileHelper.Add(file);
            await _carImageDal.AddAsync(carImage);
            return new SuccessResult("Resim eklendi");
        }

        public async Task<IResult> DeleteAsync(CarImage carImage)
        {
            new FileHelper().Delete(carImage.ImagePath);
            await _carImageDal.DeleteAsync(carImage);
            return new SuccessResult("Resim silindi");
        }

        public async Task<IDataResult<List<CarImage>>> GetAllAsync()
        {
            return new SuccessDataResult<List<CarImage>>(await _carImageDal.GetAllAsync());
        }

        public async Task<IDataResult<CarImage>> GetByIdAsync(int id)
        {
            return new SuccessDataResult<CarImage>(await _carImageDal.GetAsync(c => c.Id == id));
        }

        public async Task<IDataResult<List<CarImage>>> GetByCarIdAsync(int carId)
        {
            return new SuccessDataResult<List<CarImage>>(await _carImageDal.GetAllAsync(c => c.CarId == carId));
        }

        public async Task<IResult> UpdateAsync(CarImage carImage, IFormFile file)
        {
            var existingImage = await _carImageDal.GetAsync(p => p.Id == carImage.Id);
            carImage.ImagePath = FileHelper.Update(existingImage?.ImagePath, file);
            await _carImageDal.UpdateAsync(carImage);
            return new SuccessResult("Resim güncellendi");
        }
    }
}
