using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
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
    }
}
