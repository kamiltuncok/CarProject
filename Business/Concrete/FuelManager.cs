using Business.Constants;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace Business.Concrete
{
    public class FuelManager : IFuelService
    {
        IFuelDal _fuelDal;

        public FuelManager(IFuelDal fuelDal)
        {
            _fuelDal = fuelDal;
        }

        public IResult Add(Fuel entity)
        {
            _fuelDal.Add(entity);
            return new SuccessResult();
        }

        public IResult Delete(Fuel entity)
        {
            _fuelDal.Delete(entity);
            return new SuccessResult();
        }

        public IDataResult<List<Fuel>> GetAll()
        {
            return new SuccessDataResult<List<Fuel>>(_fuelDal.GetAll());
        }

        public IDataResult<Fuel> GetById(int id)
        {
            return new SuccessDataResult<Fuel>(_fuelDal.Get(f => f.FuelId == id));
        }

        public IDataResult<List<Fuel>> GetListById(int id)
        {
            return new SuccessDataResult<List<Fuel>>(_fuelDal.GetAll(f => f.FuelId == id));
        }

        public IResult Update(Fuel entity)
        {
            _fuelDal.Update(entity);
            return new SuccessResult();
        }
    
        public async Task<IDataResult<List<Fuel>>> GetAllAsync()
        {
            return new SuccessDataResult<List<Fuel>>(await _fuelDal.GetAllAsync(), Messages.FuelListed);
        }

        public async Task<IDataResult<Fuel>> GetByIdAsync(int id)
        {
            return new SuccessDataResult<Fuel>(await _fuelDal.GetAsync(e => e.FuelId == id));
        }

        public async Task<IResult> AddAsync(Fuel entity)
        {
            // Omit fluent validation here for brevity unless needed, or just call normal pipeline
            await _fuelDal.AddAsync(entity);
            return new SuccessResult(Messages.FuelAdded);
        }

        public async Task<IResult> UpdateAsync(Fuel entity)
        {
            await _fuelDal.UpdateAsync(entity);
            return new SuccessResult(Messages.FuelUpdated);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var entity = await _fuelDal.GetAsync(e => e.FuelId == id);
            if (entity != null)
            {
                await _fuelDal.DeleteAsync(entity);
            }
            return new SuccessResult(Messages.FuelDeleted);
        }
    }
}

