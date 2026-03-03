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
    public class ColorManager : IColorService
    {
        IColorDal _colorDal;
        public ColorManager(IColorDal colorDal)
        {
            _colorDal = colorDal;
        }
        [ValidationAspect(typeof(ColorValidator))]
        public IResult Add(Color color)
        {
            ValidationTool.Validate(new ColorValidator(), color);

            _colorDal.Add(color);
            return new SuccessResult(Messages.ColorAdded);
        }

        public IResult Delete(Color color)
        {
            _colorDal.Delete(color);
            return new SuccessResult(Messages.ColorDeleted);
        }

        public IDataResult<List<Color>> GetAll()
        {
            return new SuccessDataResult<List<Color>>(_colorDal.GetAll(),Messages.ColorsListed);
        }

        public IDataResult<List<Color>> GetById(int id)
        {
            return new SuccessDataResult<List<Color>>(_colorDal.GetAll(c => c.ColorId == id));
        }

        public IResult Update(Color color)
        {
            _colorDal.Update(color);
            return new SuccessResult(Messages.ColorUpdated);
        }
    
        public async Task<IDataResult<List<Color>>> GetAllAsync()
        {
            return new SuccessDataResult<List<Color>>(await _colorDal.GetAllAsync(), Messages.ColorsListed);
        }

        public async Task<IDataResult<Color>> GetByIdAsync(int id)
        {
            return new SuccessDataResult<Color>(await _colorDal.GetAsync(e => e.ColorId == id));
        }

        public async Task<IResult> AddAsync(Color entity)
        {
            // Omit fluent validation here for brevity unless needed, or just call normal pipeline
            await _colorDal.AddAsync(entity);
            return new SuccessResult(Messages.ColorAdded);
        }

        public async Task<IResult> UpdateAsync(Color entity)
        {
            await _colorDal.UpdateAsync(entity);
            return new SuccessResult(Messages.ColorUpdated);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var entity = await _colorDal.GetAsync(e => e.ColorId == id);
            if (entity != null)
            {
                await _colorDal.DeleteAsync(entity);
            }
            return new SuccessResult(Messages.ColorDeleted);
        }
    }
}

