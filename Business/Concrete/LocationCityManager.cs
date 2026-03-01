using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;

namespace Business.Concrete
{
    public class LocationCityManager : ILocationCityService
    {
        private readonly ILocationCityDal _locationCityDal;

        public LocationCityManager(ILocationCityDal locationCityDal)
        {
            _locationCityDal = locationCityDal;
        }

        public IResult Add(LocationCity entity)
        {
            _locationCityDal.Add(entity);
            return new SuccessResult();
        }

        public IResult Delete(LocationCity entity)
        {
            _locationCityDal.Delete(entity);
            return new SuccessResult();
        }

        public IDataResult<List<LocationCity>> GetAll()
        {
            return new SuccessDataResult<List<LocationCity>>(_locationCityDal.GetAll());
        }

        public IDataResult<LocationCity> GetById(int id)
        {
            return new SuccessDataResult<LocationCity>(_locationCityDal.Get(lc => lc.Id == id));
        }

        public IDataResult<List<LocationCity>> GetListById(int id)
        {
            return new SuccessDataResult<List<LocationCity>>(_locationCityDal.GetAll(lc => lc.Id == id));
        }

        public IResult Update(LocationCity entity)
        {
            _locationCityDal.Update(entity);
            return new SuccessResult();
        }
    }
}
