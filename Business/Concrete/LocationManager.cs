using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class LocationManager : ILocationService
    {
        ILocationDal _locationDal;

        public LocationManager(ILocationDal locationDal)
        {
            _locationDal = locationDal;
        }

        public IDataResult<List<Location>> GetAll()
        {
            return new SuccessDataResult<List<Location>>(_locationDal.GetAll(), Messages.LocationListed);
        }

        public IDataResult<Location> GetById(int id)
        {
            return new SuccessDataResult<Location>(_locationDal.Get(l => l.Id == id));
        }

        public IDataResult<List<Location>> GetListById(int id)
        {
            return new SuccessDataResult<List<Location>>(_locationDal.GetAll(l => l.Id == id));
        }

        public IResult Add(Location entity)
        {
            _locationDal.Add(entity);
            return new SuccessResult(Messages.LocationAdded);
        }

        public IResult Update(Location entity)
        {
            _locationDal.Update(entity);
            return new SuccessResult(Messages.LocationUpdated);
        }

        public IResult Delete(Location entity)
        {
            _locationDal.Delete(entity);
            return new SuccessResult(Messages.LocationDeleted);
        }

        public IDataResult<List<Location>> GetByCityId(int locationCityId)
        {
            return new SuccessDataResult<List<Location>>(
                _locationDal.GetAll(l => l.LocationCityId == locationCityId));
        }

        public async Task<IDataResult<List<Location>>> GetAllAsync()
        {
            return new SuccessDataResult<List<Location>>(await _locationDal.GetAllAsync(), Messages.LocationListed);
        }

        public async Task<IDataResult<Location>> GetByIdAsync(int id)
        {
            return new SuccessDataResult<Location>(await _locationDal.GetAsync(e => e.Id == id));
        }

        public async Task<IDataResult<List<Location>>> GetByCityIdAsync(int locationCityId)
        {
            return new SuccessDataResult<List<Location>>(
                await _locationDal.GetAllAsync(l => l.LocationCityId == locationCityId));
        }

        public async Task<IResult> AddAsync(Location entity)
        {
            await _locationDal.AddAsync(entity);
            return new SuccessResult(Messages.LocationAdded);
        }

        public async Task<IResult> UpdateAsync(Location entity)
        {
            await _locationDal.UpdateAsync(entity);
            return new SuccessResult(Messages.LocationUpdated);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var entity = await _locationDal.GetAsync(e => e.Id == id);
            if (entity != null)
            {
                await _locationDal.DeleteAsync(entity);
            }
            return new SuccessResult(Messages.LocationDeleted);
        }
    }
}
