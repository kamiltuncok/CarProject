using Business.Constants;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    
        public async Task<IDataResult<List<LocationCity>>> GetAllAsync()
        {
            return new SuccessDataResult<List<LocationCity>>(await _locationCityDal.GetAllAsync(), Messages.LocationCityListed);
        }

        public async Task<IDataResult<LocationCity>> GetByIdAsync(int id)
        {
            return new SuccessDataResult<LocationCity>(await _locationCityDal.GetAsync(e => e.Id == id));
        }

        public async Task<IResult> AddAsync(LocationCity entity)
        {
            // Omit fluent validation here for brevity unless needed, or just call normal pipeline
            await _locationCityDal.AddAsync(entity);
            return new SuccessResult(Messages.LocationCityAdded);
        }

        public async Task<IResult> UpdateAsync(LocationCity entity)
        {
            await _locationCityDal.UpdateAsync(entity);
            return new SuccessResult(Messages.LocationCityUpdated);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var entity = await _locationCityDal.GetAsync(e => e.Id == id);
            if (entity != null)
            {
                await _locationCityDal.DeleteAsync(entity);
            }
            return new SuccessResult(Messages.LocationCityDeleted);
        }
    }
}


