using Core.Entities.Concrete;
using Core.Utilities;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password);
        IDataResult<User> Login(UserForLoginDto userForLoginDto);
        IResult UserExists(string email);
        IDataResult<AccessToken> CreateAccessToken(User user);
        IDataResult<User> UpdatePassword(UserForPasswordDto userForPasswordDto, string password);

        IDataResult<CorporateUser> RegisterForCorporate(CorporateUserForRegisterDto corporateUserForRegisterDto, string password);
        IDataResult<CorporateUser> LoginForCorporateUser(UserForLoginDto userForLoginDto);
        IResult CorporateUserExists(string email);
        IDataResult<AccessToken> CreateAccessTokenForCorporate(CorporateUser corporateUser);
        IDataResult<CorporateUser> UpdateCorporatePassword(UserForPasswordDto userForPasswordDto, string password);
        IDataResult<User> RegisterAdmin(UserForRegisterDto userForRegisterDto, string password);
    }
}
