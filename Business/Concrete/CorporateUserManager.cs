using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class CorporateUserManager : ICorporateUserService
    {
        ICorporateUserDal _corporateUserDal;

        public CorporateUserManager(ICorporateUserDal corporateUserDal)
        {
            _corporateUserDal = corporateUserDal;
        }

        public void Add(CorporateUser corporateUser)
        {
            _corporateUserDal.Add(corporateUser);
        }

        public IResult Delete(CorporateUser corporateUser)
        {
            _corporateUserDal.Delete(corporateUser);
            return new SuccessResult(Messages.UserDeleted);
        }

        public IDataResult<CorporateUser> GetByEmailWithResult(string email)
        {
            var result = GetByMail(email);
            if (result == null)
            {
                return new ErrorDataResult<CorporateUser>(Messages.UserNotFound);
            }
            return new SuccessDataResult<CorporateUser>(result);
        }

        public IDataResult<CorporateUser> GetById(int userId)
        {
            return new SuccessDataResult<CorporateUser>(_corporateUserDal.Get(u => u.Id == userId));
        }

        public CorporateUser GetByMail(string email)
        {
            return _corporateUserDal.Get(u => u.Email == email);
        }

        public List<OperationClaim> GetClaims(CorporateUser corporateUser)
        {
            return _corporateUserDal.GetClaims(corporateUser);
        }

        public IResult Update(CorporateUser corporateUser)
        {
            _corporateUserDal.Update(corporateUser);
            return new SuccessResult(Messages.UserUpdated);
        }
    }
}
