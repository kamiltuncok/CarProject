using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class IndividualCustomerManager : IIndividualCustomerService
    {
        private readonly IIndividualCustomerDal _individualCustomerDal;

        public IndividualCustomerManager(IIndividualCustomerDal individualCustomerDal)
        {
            _individualCustomerDal = individualCustomerDal;
        }

        public IResult Add(IndividualCustomer entity)
        {
            _individualCustomerDal.Add(entity);
            return new SuccessResult("Bireysel m??teri eklendi");
        }

        public IResult Delete(IndividualCustomer entity)
        {
            _individualCustomerDal.Delete(entity);
            return new SuccessResult("Bireysel m??teri silindi");
        }

        public IDataResult<List<IndividualCustomer>> GetAll()
        {
            return new SuccessDataResult<List<IndividualCustomer>>(_individualCustomerDal.GetAll());
        }

        public IDataResult<IndividualCustomer> GetById(int id)
        {
            return new SuccessDataResult<IndividualCustomer>(_individualCustomerDal.Get(c => c.Id == id));
        }

        public IDataResult<List<IndividualCustomer>> GetListById(int id)
        {
            return new SuccessDataResult<List<IndividualCustomer>>(_individualCustomerDal.GetAll(c => c.Id == id));
        }

        public IResult Update(IndividualCustomer entity)
        {
            _individualCustomerDal.Update(entity);
            return new SuccessResult("Bireysel m??teri g?ncellendi");
        }

        public IDataResult<IndividualCustomer> AddCustomer(IndividualCustomer entity)
        {
            _individualCustomerDal.Add(entity);
            return new SuccessDataResult<IndividualCustomer>(entity, "Eklendi");
        }

        public async Task<IResult> DeleteAsync(IndividualCustomer customer)
        {
            await _individualCustomerDal.DeleteAsync(customer);
            return new SuccessResult("Bireysel musteri silindi");
        }
        public async Task<IResult> AddAsync(IndividualCustomer customer)
        {
            await _individualCustomerDal.AddAsync(customer);
            return new SuccessResult("Bireysel m??teri eklendi");
        }
        public async Task<IDataResult<IndividualCustomer>> GetByIdAsync(int id)
        {
            return new SuccessDataResult<IndividualCustomer>(await _individualCustomerDal.GetAsync(c => c.Id == id));
        }
        public async Task<IDataResult<List<IndividualCustomer>>> GetAllAsync()
        {
            return new SuccessDataResult<List<IndividualCustomer>>(await _individualCustomerDal.GetAllAsync());
        }
        public async Task<IResult> UpdateAsync(IndividualCustomer customer)
        {
            await _individualCustomerDal.UpdateAsync(customer);
            return new SuccessResult("Bireysel m??teri g?ncellendi");
        }
    }
}


