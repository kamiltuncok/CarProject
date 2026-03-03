using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CorporateCustomerManager : ICorporateCustomerService
    {
        private readonly ICorporateCustomerDal _corporateCustomerDal;

        public CorporateCustomerManager(ICorporateCustomerDal corporateCustomerDal)
        {
            _corporateCustomerDal = corporateCustomerDal;
        }

        public IResult Add(CorporateCustomer entity)
        {
            _corporateCustomerDal.Add(entity);
            return new SuccessResult("Kurumsal m??teri eklendi");
        }

        public IResult Delete(CorporateCustomer entity)
        {
            _corporateCustomerDal.Delete(entity);
            return new SuccessResult("Kurumsal m??teri silindi");
        }

        public IDataResult<List<CorporateCustomer>> GetAll()
        {
            return new SuccessDataResult<List<CorporateCustomer>>(_corporateCustomerDal.GetAll());
        }

        public IDataResult<CorporateCustomer> GetById(int id)
        {
            return new SuccessDataResult<CorporateCustomer>(_corporateCustomerDal.Get(c => c.Id == id));
        }

        public IDataResult<List<CorporateCustomer>> GetListById(int id)
        {
            return new SuccessDataResult<List<CorporateCustomer>>(_corporateCustomerDal.GetAll(c => c.Id == id));
        }

        public IResult Update(CorporateCustomer entity)
        {
            _corporateCustomerDal.Update(entity);
            return new SuccessResult("Kurumsal m??teri g?ncellendi");
        }

        public IDataResult<CorporateCustomer> AddCustomer(CorporateCustomer entity)
        {
            _corporateCustomerDal.Add(entity);
            return new SuccessDataResult<CorporateCustomer>(entity, "Eklendi");
        }

        public async Task<IResult> DeleteAsync(CorporateCustomer customer)
        {
            await _corporateCustomerDal.DeleteAsync(customer);
            return new SuccessResult("Kurumsal musteri silindi");
        }
        public async Task<IResult> AddAsync(CorporateCustomer customer)
        {
            await _corporateCustomerDal.AddAsync(customer);
            return new SuccessResult("Kurumsal m??teri eklendi");
        }
        public async Task<IDataResult<CorporateCustomer>> GetByIdAsync(int id)
        {
            return new SuccessDataResult<CorporateCustomer>(await _corporateCustomerDal.GetAsync(c => c.Id == id));
        }
        public async Task<IDataResult<List<CorporateCustomer>>> GetAllAsync()
        {
            return new SuccessDataResult<List<CorporateCustomer>>(await _corporateCustomerDal.GetAllAsync());
        }
        public async Task<IResult> UpdateAsync(CorporateCustomer customer)
        {
            await _corporateCustomerDal.UpdateAsync(customer);
            return new SuccessResult("Kurumsal m??teri g?ncellendi");
        }
    }
}


