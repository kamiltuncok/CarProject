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
    public class SegmentManager : ISegmentService
    {
        ISegmentDal _segmentDal;

        public SegmentManager(ISegmentDal segmentDal)
        {
            _segmentDal = segmentDal;
        }

        public IResult Add(Segment entity)
        {
            _segmentDal.Add(entity);
            return new SuccessResult();
        }

        public IResult Delete(Segment entity)
        {
            _segmentDal.Delete(entity);
            return new SuccessResult();
        }

        public IDataResult<List<Segment>> GetAll()
        {
            return new SuccessDataResult<List<Segment>>(_segmentDal.GetAll());
        }

        public IDataResult<Segment> GetById(int id)
        {
            return new SuccessDataResult<Segment>(_segmentDal.Get(s => s.SegmentId == id));
        }

        public IDataResult<List<Segment>> GetListById(int id)
        {
            return new SuccessDataResult<List<Segment>>(_segmentDal.GetAll(s => s.SegmentId == id));
        }

        public IResult Update(Segment entity)
        {
            _segmentDal.Update(entity);
            return new SuccessResult();
        }
    
        public async Task<IDataResult<List<Segment>>> GetAllAsync()
        {
            return new SuccessDataResult<List<Segment>>(await _segmentDal.GetAllAsync(), Messages.SegmentListed);
        }

        public async Task<IDataResult<Segment>> GetByIdAsync(int id)
        {
            return new SuccessDataResult<Segment>(await _segmentDal.GetAsync(e => e.SegmentId == id));
        }

        public async Task<IResult> AddAsync(Segment entity)
        {
            // Omit fluent validation here for brevity unless needed, or just call normal pipeline
            await _segmentDal.AddAsync(entity);
            return new SuccessResult(Messages.SegmentAdded);
        }

        public async Task<IResult> UpdateAsync(Segment entity)
        {
            await _segmentDal.UpdateAsync(entity);
            return new SuccessResult(Messages.SegmentUpdated);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var entity = await _segmentDal.GetAsync(e => e.SegmentId == id);
            if (entity != null)
            {
                await _segmentDal.DeleteAsync(entity);
            }
            return new SuccessResult(Messages.SegmentDeleted);
        }
    }
}

