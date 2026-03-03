using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        ICustomerDal _customerDal;
        public CustomerManager(ICustomerDal customerDal)
        {
            _customerDal = customerDal;
        }
        [ValidationAspect(typeof(CustomerValidator))]
        public IResult Add(Customer customer)
        {
            ValidationTool.Validate(new CustomerValidator(), customer);

            _customerDal.Add(customer);
            return new SuccessResult(Messages.CustomerAdded);
        }

        public IResult Delete(Customer customer)
        {
            _customerDal.Delete(customer);
            return new SuccessResult(Messages.CustomerDeleted);
        }

        public IDataResult<List<Customer>> GetAll()
        {
            return new SuccessDataResult<List<Customer>>(_customerDal.GetAll(), Messages.CustomerListed);
        }

        public IDataResult<Customer> GetById(int customerid)
        {
            return new SuccessDataResult<Customer>(_customerDal.Get(c => c.Id == customerid), Messages.CustomerListed);
        }

        public IDataResult<CustomerDetailDto> GetCustomerDetailById(int id)
        {
            return new SuccessDataResult<CustomerDetailDto>(_customerDal.GetCustomerDetailById(id));
        }

        public IDataResult<List<CustomerDetailDto>> GetCustomerDetails()
        {
            return new SuccessDataResult<List<CustomerDetailDto>>(_customerDal.GetCustomerDetails());
        }

        public IResult Update(Customer customer)
        {
            _customerDal.Update(customer);
            return new SuccessResult(Messages.CustomerUpdated);
        }
        public async Task<IResult> AddAsync(Customer customer)
        {
            ValidationTool.Validate(new CustomerValidator(), customer);
            await _customerDal.AddAsync(customer);
            return new SuccessResult(Messages.CustomerAdded);
        }

        public async Task<IResult> DeleteAsync(Customer customer)
        {
            await _customerDal.DeleteAsync(customer);
            return new SuccessResult(Messages.CustomerDeleted);
        }

        public async Task<IDataResult<List<Customer>>> GetAllAsync()
        {
            return new SuccessDataResult<List<Customer>>(await _customerDal.GetAllAsync(), Messages.CustomerListed);
        }

        public async Task<IDataResult<Customer>> GetByIdAsync(int customerid)
        {
            return new SuccessDataResult<Customer>(await _customerDal.GetAsync(c => c.Id == customerid), Messages.CustomerListed);
        }

        public async Task<IDataResult<CustomerDetailDto>> GetCustomerDetailByIdAsync(int id)
        {
            return new SuccessDataResult<CustomerDetailDto>(await _customerDal.GetCustomerDetailByIdAsync(id));
        }

        public async Task<IDataResult<List<CustomerDetailDto>>> GetCustomerDetailsAsync()
        {
            return new SuccessDataResult<List<CustomerDetailDto>>(await _customerDal.GetCustomerDetailsAsync());
        }

        public async Task<IResult> UpdateAsync(Customer customer)
        {
            await _customerDal.UpdateAsync(customer);
            return new SuccessResult(Messages.CustomerUpdated);
        }
    }
}

