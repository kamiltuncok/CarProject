using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
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
    }
}
