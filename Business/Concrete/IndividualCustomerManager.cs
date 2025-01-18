using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class IndividualCustomerManager : IIndividualCustomerService
    {
        IIndividualCustomerDal _customerDal;

        public IndividualCustomerManager(IIndividualCustomerDal customerDal)
        {
            _customerDal = customerDal;
        }

        public IResult Add(IndividualCustomer entity)
        {
            throw new NotImplementedException();
        }

        [ValidationAspect(typeof(CustomerValidator))]
        public IDataResult<IndividualCustomer> AddCustomer(IndividualCustomer entity)
        {
            _customerDal.Add(entity);
            return new SuccessDataResult<IndividualCustomer>(entity, Messages.CustomerAdded);
}

        public IResult Delete(IndividualCustomer entity)
        {
            _customerDal.Delete(entity);
            return new SuccessResult(Messages.CustomerDeleted);
        }

        public IDataResult<List<IndividualCustomer>> GetAll()
        {
            return new SuccessDataResult<List<IndividualCustomer>>(_customerDal.GetAll(), Messages.CustomerListed);
        }

        public IDataResult<IndividualCustomer> GetById(int id)
        {
            return new SuccessDataResult<IndividualCustomer>(_customerDal.Get(c => c.CustomerId == id), Messages.CustomerListed);
        }

        public IDataResult<List<IndividualCustomer>> GetListById(int id)
        {
            return new SuccessDataResult<List<IndividualCustomer>>(_customerDal.GetAll(c => c.CustomerId == id), Messages.CustomerListed);
        }

        public IResult Update(IndividualCustomer entity)
        {
            _customerDal.Delete(entity);
            return new SuccessResult(Messages.CustomerUpdated);
        }
    }
}
