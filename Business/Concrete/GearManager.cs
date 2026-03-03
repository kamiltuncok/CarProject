using Business.Constants;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Business.Concrete
{
    public class GearManager : IGearService
    {
        IGearDal _gearDal;

        public GearManager(IGearDal gearDal)
        {
            _gearDal = gearDal;
        }

        public IResult Add(Gear entity)
        {
            _gearDal.Add(entity);
            return new SuccessResult();
        }

        public IResult Delete(Gear entity)
        {
            _gearDal.Delete(entity);
            return new SuccessResult();
        }

        public IDataResult<List<Gear>> GetAll()
        {
            return new SuccessDataResult<List<Gear>>(_gearDal.GetAll());
        }

        public IDataResult<Gear> GetById(int id)
        {
            return new SuccessDataResult<Gear>(_gearDal.Get(g=>g.GearId == id));
        }

        public IDataResult<List<Gear>> GetListById(int id)
        {
            return new SuccessDataResult<List<Gear>>(_gearDal.GetAll(g=>g.GearId==id));
        }

        public IResult Update(Gear entity)
        {
            _gearDal.Update(entity);
            return new SuccessResult();
        }
    
        public async Task<IDataResult<List<Gear>>> GetAllAsync()
        {
            return new SuccessDataResult<List<Gear>>(await _gearDal.GetAllAsync(), Messages.GearListed);
        }

        public async Task<IDataResult<Gear>> GetByIdAsync(int id)
        {
            return new SuccessDataResult<Gear>(await _gearDal.GetAsync(e => e.GearId == id));
        }

        public async Task<IResult> AddAsync(Gear entity)
        {
            // Omit fluent validation here for brevity unless needed, or just call normal pipeline
            await _gearDal.AddAsync(entity);
            return new SuccessResult(Messages.GearAdded);
        }

        public async Task<IResult> UpdateAsync(Gear entity)
        {
            await _gearDal.UpdateAsync(entity);
            return new SuccessResult(Messages.GearUpdated);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var entity = await _gearDal.GetAsync(e => e.GearId == id);
            if (entity != null)
            {
                await _gearDal.DeleteAsync(entity);
            }
            return new SuccessResult(Messages.GearDeleted);
        }
    }
}

