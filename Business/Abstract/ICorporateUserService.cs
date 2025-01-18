using Core.Entities.Concrete;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface ICorporateUserService
    {
        List<OperationClaim> GetClaims(CorporateUser corporateUser);
        void Add(CorporateUser corporateUser);
        CorporateUser GetByMail(string email);
        IDataResult<CorporateUser> GetByEmailWithResult(string email);
        IDataResult<CorporateUser> GetById(int userId);
        IResult Update(CorporateUser corporateUser);
        IResult Delete(CorporateUser corporateUser);
    }
}
