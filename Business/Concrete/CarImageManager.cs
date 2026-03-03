using Business.Abstract;
using Business.Constants;
using Core.Utilities.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        ICarImageDal _carImageDal;

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

        public IDataResult<CarImage> GetById(int carimageid)
        {
            return new SuccessDataResult<CarImage>(_carImageDal.Get(c => c.CarImageId == carimageid));
        }

        public IDataResult<CarImage> GetCarImageByColorAndBrandId(int brandId, int colorId)
        {
            return new SuccessDataResult<CarImage>(_carImageDal.Get(i => i.BrandId == brandId && i.ColorId == colorId));
        }

        public IResult Update(CarImage carImage, IFormFile file)
        {
            carImage.ImagePath = FileHelper.Update(_carImageDal.Get(p => p.CarImageId == carImage.CarImageId).ImagePath, file);
            _carImageDal.Update(carImage);
            return new SuccessResult("Resim g?ncellendi");
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

        public async Task<IDataResult<CarImage>> GetByIdAsync(int carimageid)
        {
            return new SuccessDataResult<CarImage>(await _carImageDal.GetAsync(c => c.CarImageId == carimageid));
        }

        public async Task<IResult> UpdateAsync(CarImage carImage, IFormFile file)
        {
            var existingImage = await _carImageDal.GetAsync(p => p.CarImageId == carImage.CarImageId);
            carImage.ImagePath = FileHelper.Update(existingImage.ImagePath, file);
            await _carImageDal.UpdateAsync(carImage);
            return new SuccessResult("Resim g?ncellendi");
        }

        public async Task<IDataResult<CarImage>> GetCarImageByColorAndBrandIdAsync(int brandId, int colorId)
        {
            return new SuccessDataResult<CarImage>(await _carImageDal.GetAsync(i => i.BrandId == brandId && i.ColorId == colorId)); 
        }
    }
}
