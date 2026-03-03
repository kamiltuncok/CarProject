using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public List<OperationClaim> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }

        public void Add(User user)
        {
            _userDal.Add(user);
        }

        public IResult Update(User user)
        {
            _userDal.Update(user);
            return new SuccessResult(Messages.UserUpdated);
        }

        public IResult UpdateUserNames(User user)
        {
            // Names are resolved polymorphically from Customer records now.
            return new SuccessResult(Messages.UserUpdated);
        }

        public IResult Delete(User user)
        {
            _userDal.Delete(user);
            return new SuccessResult(Messages.UserDeleted);
        }

        public User GetByMail(string email)
        {
            return _userDal.Get(u => u.Email == email);
        }

        public List<User> GetAll()
        {
            return _userDal.GetAll();
        }

        public IDataResult<User> GetByEmailWithResult(string email)
        {
            var result = GetByMail(email);
            if (result == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }
            return new SuccessDataResult<User>(result);
        }

        public IDataResult<User> GetById(int userId)
        {
            return new SuccessDataResult<User>(_userDal.Get(u => u.Id == userId));
        }

        public async Task<List<OperationClaim>> GetClaimsAsync(User user)
        {
            return await _userDal.GetClaimsAsync(user);
        }

        public async Task AddAsync(User user)
        {
            await _userDal.AddAsync(user);
        }

        public async Task<IResult> UpdateAsync(User user)
        {
            await _userDal.UpdateAsync(user);
            return new SuccessResult(Messages.UserUpdated);
        }

        public async Task<IResult> UpdateUserNamesAsync(User user)
        {
            return new SuccessResult(Messages.UserUpdated);
        }

        public async Task<IResult> DeleteAsync(User user)
        {
            await _userDal.DeleteAsync(user);
            return new SuccessResult(Messages.UserDeleted);
        }

        public async Task<User> GetByMailAsync(string email)
        {
            return await _userDal.GetAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userDal.GetAllAsync();
        }

        public async Task<IDataResult<User>> GetByEmailWithResultAsync(string email)
        {
            var result = await GetByMailAsync(email);
            if (result == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }
            return new SuccessDataResult<User>(result);
        }

        public async Task<IDataResult<User>> GetByIdAsync(int userId)
        {
            return new SuccessDataResult<User>(await _userDal.GetAsync(u => u.Id == userId));
        }
    }
}

