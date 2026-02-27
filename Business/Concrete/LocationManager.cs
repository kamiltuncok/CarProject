using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class LocationManager : ILocationService
    {
        ILocationDal _locationDal;

        public LocationManager(ILocationDal locationDal)
        {
            _locationDal = locationDal;
        }

        public IResult Add(Location entity)
        {
            _locationDal.Add(entity);
            return new SuccessResult();
        }

        public IResult Delete(Location entity)
        {
            _locationDal.Delete(entity);
            return new SuccessResult();
        }

        public IDataResult<List<Location>> GetAll()
        {
            return new SuccessDataResult<List<Location>>(_locationDal.GetAll());
        }

        public IDataResult<Location> GetById(int id)
        {
            return new SuccessDataResult<Location>(_locationDal.Get(l => l.Id == id));
        }

        public IDataResult<List<Location>> GetByLocationCity(string locationCity)
        {
            return new SuccessDataResult<List<Location>>(_locationDal.GetAll(l => l.LocationCity == locationCity));
        }

        public IDataResult<List<Location>> GetListById(int id)
        {
            return new SuccessDataResult<List<Location>>(_locationDal.GetAll(l => l.Id == id));
        }

        public IResult Update(Location entity)
        {
            _locationDal.Update(entity);
            return new SuccessResult();
        }
    }
}
