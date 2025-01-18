using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities;
using Core.Utilities.Business;
using Core.Utilities.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        ICarImageDal _carImageDal;
        IFileHelper _fileHelper;
        public CarImageManager(ICarImageDal carImageDal, IFileHelper fileHelper)
        {
            _carImageDal = carImageDal;
            _fileHelper = fileHelper;
        }

        [ValidationAspect(typeof(CarImageValidator))]
        public IResult Add(IFormFile file, CarImage carImage)
        {
            carImage.ImagePath = _fileHelper.Upload(file, PathConstants.ImagesPath);
            _carImageDal.Add(carImage);
            return new SuccessResult("Resim başarıyla yüklendi");
        }


        public IResult Delete(CarImage carImage)
        {
            File.Delete(carImage.ImagePath);
            _carImageDal.Add(carImage);
            return new SuccessResult(Messages.CarImageDeleted);
        }

        public IDataResult<List<CarImage>> GetAll()
        {
            return new SuccessDataResult<List<CarImage>>(_carImageDal.GetAll(), Messages.CarImageListed);
        }

        public IDataResult<CarImage> GetById(int carimageid)
        {
            return new SuccessDataResult<CarImage>(_carImageDal.Get(c => c.CarImageId == carimageid));
        }

        public IDataResult<CarImage> GetCarImageByColorAndBrandId(int brandId, int colorId)
        {
            return new SuccessDataResult<CarImage>(_carImageDal.Get(c => c.BrandId == brandId && c.ColorId == colorId));
        }

        [ValidationAspect(typeof(CarImageValidator))]
        public IResult Update(CarImage carImage, IFormFile file)
        {
            //  carImage.ImagePath = FileHelper.Update(_carImageDal.Get(c => c.Id == carImage.Id).ImagePath, file);
            _carImageDal.Add(carImage);
            return new SuccessResult(Messages.CarImageUpdated);
        }

    }
    //public IDataResult<List<CarImage>> GetCarImagesByCarId(int carId)
    //{
    //    var data = _carImageDal.GetAll(cI => cI.CarId == carId);
    //    if (data.Count == 0)
    //    {
    //        data.Add(new CarImage
    //        {
    //            CarId = carId,
    //            ImagePath = "/Images/volvo.jpg"
    //        });
    //    }
    //    return new SuccessDataResult<List<CarImage>>(data);
    //}


}



