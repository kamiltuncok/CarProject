using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class CorporateCustomerManager : ICorporateCustomerService
    {
        ICorporateCustomerDal _customerDal;

        public CorporateCustomerManager(ICorporateCustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        public IResult Add(CorporateCustomer entity)
        {
            _customerDal.Add(entity);
            return new SuccessResult(Messages.CustomerAdded);
        }

        public IDataResult<CorporateCustomer> AddCustomer(CorporateCustomer entity)
        {
            _customerDal.Add(entity);
            return new SuccessDataResult<CorporateCustomer>(entity, Messages.CustomerAdded);
        }

        public IResult Delete(CorporateCustomer entity)
        {
            _customerDal.Delete(entity);
            return new SuccessResult(Messages.CustomerDeleted);
        }

        public IDataResult<List<CorporateCustomer>> GetAll()
        {
            return new SuccessDataResult<List<CorporateCustomer>>(_customerDal.GetAll(), Messages.CustomerListed);
        }

        public IDataResult<CorporateCustomer> GetById(int id)
        {
            return new SuccessDataResult<CorporateCustomer>(_customerDal.Get(c => c.CustomerId == id), Messages.CustomerListed);
        }

        public IDataResult<List<CorporateCustomer>> GetListById(int id)
        {
            return new SuccessDataResult<List<CorporateCustomer>>(_customerDal.GetAll(c => c.CustomerId == id), Messages.CustomerListed);
        }

        public IResult Update(CorporateCustomer entity)
        {
            _customerDal.Update(entity);
            return new SuccessResult(Messages.CustomerUpdated);
        }
    }
}
