using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
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
    }
}
