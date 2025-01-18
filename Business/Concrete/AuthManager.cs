using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.DTOs;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        private ICorporateUserService _corporateUserService;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, ICorporateUserService corporateUserService)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _corporateUserService = corporateUserService;
        }

        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                IdentityNumber = userForRegisterDto.IdentityNumber,
                PhoneNumber = userForRegisterDto.PhoneNumber,
                Address = userForRegisterDto.Address,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
                CustomerType = CustomerType.Individual
            };
            _userService.Add(user);
            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }

        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            return new SuccessDataResult<User>(userToCheck, Messages.SuccessfulLogin);
        }

        public IResult UserExists(string email)
        {
            if (_userService.GetByMail(email) != null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        public IDataResult<User> UpdatePassword(UserForPasswordDto userForPasswordDto, string newPassword)
        {
            //Business Rules
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);
            var updatedUser = _userService.GetById(userForPasswordDto.UserId).Data;

            if (!HashingHelper.VerifyPasswordHash(userForPasswordDto.OldPassword, updatedUser.PasswordHash, updatedUser.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            if (!userForPasswordDto.NewPassword.Equals(userForPasswordDto.RepeatNewPassword))
            {
                return new ErrorDataResult<User>("Şifre tekrarı yanlış!");
            };

            updatedUser.PasswordHash = passwordHash;
            updatedUser.PasswordSalt = passwordSalt;

            _userService.Update(updatedUser);
            return new SuccessDataResult<User>(updatedUser, Messages.UserPasswordUpdated);
        }

        public IDataResult<CorporateUser> RegisterForCorporate(CorporateUserForRegisterDto corporateUserForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var corporateUser = new CorporateUser
            {
                Email = corporateUserForRegisterDto.Email,
                CompanyName = corporateUserForRegisterDto.CompanyName,
                TaxNumber = corporateUserForRegisterDto.TaxNumber,
                PhoneNumber = corporateUserForRegisterDto.PhoneNumber,
                Address = corporateUserForRegisterDto.Address,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
                CustomerType=CustomerType.Corporate
            };
            _corporateUserService.Add(corporateUser);
            return new SuccessDataResult<CorporateUser>(corporateUser, Messages.UserRegistered);
        }

        public IDataResult<CorporateUser> LoginForCorporateUser(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _corporateUserService.GetByMail(userForLoginDto.Email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<CorporateUser>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<CorporateUser>(Messages.PasswordError);
            }

            return new SuccessDataResult<CorporateUser>(userToCheck, Messages.SuccessfulLogin);
        }

        public IDataResult<AccessToken> CreateAccessTokenForCorporate(CorporateUser corporateUser)
        {
            var claims = _corporateUserService.GetClaims(corporateUser);
            var accessToken = _tokenHelper.CreateTokenForCorporate(corporateUser, claims);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        public IDataResult<CorporateUser> UpdateCorporatePassword(UserForPasswordDto userForPasswordDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var updatedUser = _corporateUserService.GetById(userForPasswordDto.UserId).Data;

            if (!HashingHelper.VerifyPasswordHash(userForPasswordDto.OldPassword, updatedUser.PasswordHash, updatedUser.PasswordSalt))
            {
                return new ErrorDataResult<CorporateUser>(Messages.PasswordError);
            }

            if (!userForPasswordDto.NewPassword.Equals(userForPasswordDto.RepeatNewPassword))
            {
                return new ErrorDataResult<CorporateUser>("Şifre tekrarı yanlış!");
            };

            updatedUser.PasswordHash = passwordHash;
            updatedUser.PasswordSalt = passwordSalt;

            _corporateUserService.Update(updatedUser);
            return new SuccessDataResult<CorporateUser>(updatedUser, Messages.UserPasswordUpdated);
        }

        public IResult CorporateUserExists(string email)
        {
            if (_corporateUserService.GetByMail(email) != null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }
    }
}
