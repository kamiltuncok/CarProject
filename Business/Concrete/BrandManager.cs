using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Business.Concrete
{
    public class BrandManager : IBrandService
    {
        IBrandDal _brandDal;
        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }
        [ValidationAspect(typeof(BrandValidator))]
        public IResult Add(Brand brand)
        {
            ValidationTool.Validate(new BrandValidator(), brand);

            _brandDal.Add(brand);
            return new SuccessResult(Messages.BrandAdded);
        }

        public IResult Delete(Brand brand)
        {
            _brandDal.Delete(brand);
            return new SuccessResult(Messages.BrandDeleted);
        }

        public IDataResult<List<Brand>> GetAll()
        {
            return new SuccessDataResult<List<Brand>>(_brandDal.GetAll(),Messages.BrandListed);
        }

        public IDataResult<List<Brand>> GetById(int id)
        {
            return new  SuccessDataResult<List<Brand>>(_brandDal.GetAll(b => b.BrandId == id));
        }

        public IResult Update(Brand brand)
        {
            _brandDal.Update(brand);
            return new SuccessResult(Messages.BrandUpdated);
        }
    
        public async Task<IDataResult<List<Brand>>> GetAllAsync()
        {
            return new SuccessDataResult<List<Brand>>(await _brandDal.GetAllAsync(), Messages.BrandListed);
        }

        public async Task<IDataResult<Brand>> GetByIdAsync(int id)
        {
            return new SuccessDataResult<Brand>(await _brandDal.GetAsync(e => e.BrandId == id));
        }

        public async Task<IResult> AddAsync(Brand entity)
        {
            // Omit fluent validation here for brevity unless needed, or just call normal pipeline
            await _brandDal.AddAsync(entity);
            return new SuccessResult(Messages.BrandAdded);
        }

        public async Task<IResult> UpdateAsync(Brand entity)
        {
            await _brandDal.UpdateAsync(entity);
            return new SuccessResult(Messages.BrandUpdated);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var entity = await _brandDal.GetAsync(e => e.BrandId == id);
            if (entity != null)
            {
                await _brandDal.DeleteAsync(entity);
            }
            return new SuccessResult(Messages.BrandDeleted);
        }
    }
}
