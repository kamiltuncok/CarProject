using Core.Entities.Concrete;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        List<OperationClaim> GetClaims(User user);
        Task<List<OperationClaim>> GetClaimsAsync(User user);
        void Add(User user);
        Task AddAsync(User user);
        User GetByMail(string email);
        Task<User> GetByMailAsync(string email);
        IDataResult<User> GetByEmailWithResult(string email);
        Task<IDataResult<User>> GetByEmailWithResultAsync(string email);
        IDataResult<User> GetById(int userId);
        Task<IDataResult<User>> GetByIdAsync(int userId);
        IResult Update(User user);
        Task<IResult> UpdateAsync(User user);
        IResult UpdateUserNames(User user);
        Task<IResult> UpdateUserNamesAsync(User user);
        IResult Delete(User user);
        Task<IResult> DeleteAsync(User user);
        List<User> GetAll();
        Task<List<User>> GetAllAsync();
    }
}

