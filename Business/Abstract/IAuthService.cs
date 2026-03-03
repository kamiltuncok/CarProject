using Core.Entities.Concrete;
using Core.Utilities;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> RegisterIndividual(IndividualRegisterDto registerDto);
        Task<IDataResult<User>> RegisterIndividualAsync(IndividualRegisterDto registerDto);
        IDataResult<User> RegisterCorporate(CorporateRegisterDto registerDto);
        Task<IDataResult<User>> RegisterCorporateAsync(CorporateRegisterDto registerDto);
        IDataResult<User> RegisterAdmin(IndividualRegisterDto registerDto);
        Task<IDataResult<User>> RegisterAdminAsync(IndividualRegisterDto registerDto);

        IDataResult<User> Login(UserForLoginDto userForLoginDto);
        Task<IDataResult<User>> LoginAsync(UserForLoginDto userForLoginDto);
        IResult UserExists(string email);
        Task<IResult> UserExistsAsync(string email);
        IDataResult<AccessToken> CreateAccessToken(User user);
        Task<IDataResult<AccessToken>> CreateAccessTokenAsync(User user);
        IDataResult<User> UpdatePassword(UserForPasswordDto userForPasswordDto, string password);
        Task<IDataResult<User>> UpdatePasswordAsync(UserForPasswordDto userForPasswordDto, string password);
    }
}

