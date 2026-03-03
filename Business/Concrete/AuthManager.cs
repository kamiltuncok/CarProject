using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private readonly ICustomerService _customerService;
        private readonly IIndividualCustomerService _individualCustomerService;
        private readonly ICorporateCustomerService _corporateCustomerService;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper, 
            ICustomerService customerService, 
            IIndividualCustomerService individualCustomerService, 
            ICorporateCustomerService corporateCustomerService)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _customerService = customerService;
            _individualCustomerService = individualCustomerService;
            _corporateCustomerService = corporateCustomerService;
        }

        public IDataResult<User> RegisterIndividual(IndividualRegisterDto registerDto)
        {
            if (_userService.GetByMail(registerDto.Email) != null)
                return new ErrorDataResult<User>(Messages.UserAlreadyExists);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);

            var individualCustomer = new IndividualCustomer
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                IdentityNumber = registerDto.IdentityNumber,
                PhoneNumber = registerDto.PhoneNumber,
                CreatedDate = DateTime.Now
            };

            _individualCustomerService.Add(individualCustomer);

            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
                CustomerId = individualCustomer.Id
            };
            
            _userService.Add(user);
            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }

        public IDataResult<User> RegisterCorporate(CorporateRegisterDto registerDto)
        {
            if (_userService.GetByMail(registerDto.Email) != null)
                return new ErrorDataResult<User>(Messages.UserAlreadyExists);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);

            var corporateCustomer = new CorporateCustomer
            {
                CompanyName = registerDto.CompanyName,
                TaxNumber = registerDto.TaxNumber,
                PhoneNumber = registerDto.PhoneNumber,
                CreatedDate = DateTime.Now
            };

            _corporateCustomerService.Add(corporateCustomer);

            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
                CustomerId = corporateCustomer.Id
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

            var customer = _customerService.GetById(user.CustomerId).Data;
            string displayName = "Unknown";

            if (customer is IndividualCustomer individual)
            {
                displayName = $"{individual.FirstName} {individual.LastName}";
            }
            else if (customer is CorporateCustomer corporate)
            {
                displayName = corporate.CompanyName;
            }

            var additionalClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, displayName),
                new Claim("CustomerId", user.CustomerId.ToString())
            };

            var accessToken = _tokenHelper.CreateToken(user, claims, additionalClaims);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        public IDataResult<User> UpdatePassword(UserForPasswordDto userForPasswordDto, string newPassword)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);
            var updatedUser = _userService.GetById(userForPasswordDto.UserId).Data;

            if (!HashingHelper.VerifyPasswordHash(userForPasswordDto.OldPassword, updatedUser.PasswordHash, updatedUser.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            if (!userForPasswordDto.NewPassword.Equals(userForPasswordDto.RepeatNewPassword))
            {
                return new ErrorDataResult<User>("Þifre tekrarý yanlýþ!");
            };

            updatedUser.PasswordHash = passwordHash;
            updatedUser.PasswordSalt = passwordSalt;

            _userService.Update(updatedUser);
            return new SuccessDataResult<User>(updatedUser, Messages.UserPasswordUpdated);
        }

        public IDataResult<User> RegisterAdmin(IndividualRegisterDto registerDto)
        {
            if (_userService.GetByMail(registerDto.Email) != null)
            {
                return new ErrorDataResult<User>(Messages.UserAlreadyExists);
            }

            // Create admin context customer (individual)
            var individualCustomer = new IndividualCustomer
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                IdentityNumber = registerDto.IdentityNumber,
                PhoneNumber = registerDto.PhoneNumber,
                CreatedDate = DateTime.Now
            };

            _individualCustomerService.Add(individualCustomer);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);

            var adminUser = new User
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
                CustomerId = individualCustomer.Id
            };

            _userService.Add(adminUser);
            return new SuccessDataResult<User>(adminUser, "Admin kullanýcýsý baþarýyla oluþturuldu");
        }
        public async Task<IDataResult<User>> RegisterIndividualAsync(IndividualRegisterDto registerDto)
        {
            if (await _userService.GetByMailAsync(registerDto.Email) != null)
                return new ErrorDataResult<User>(Messages.UserAlreadyExists);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);

            var individualCustomer = new IndividualCustomer
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                IdentityNumber = registerDto.IdentityNumber,
                PhoneNumber = registerDto.PhoneNumber,
                CreatedDate = DateTime.Now
            };

            await _individualCustomerService.AddAsync(individualCustomer);

            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
                CustomerId = individualCustomer.Id
            };
            
            await _userService.AddAsync(user);
            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }

        public async Task<IDataResult<User>> RegisterCorporateAsync(CorporateRegisterDto registerDto)
        {
            if (await _userService.GetByMailAsync(registerDto.Email) != null)
                return new ErrorDataResult<User>(Messages.UserAlreadyExists);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);

            var corporateCustomer = new CorporateCustomer
            {
                CompanyName = registerDto.CompanyName,
                TaxNumber = registerDto.TaxNumber,
                PhoneNumber = registerDto.PhoneNumber,
                CreatedDate = DateTime.Now
            };

            await _corporateCustomerService.AddAsync(corporateCustomer);

            var user = new User
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
                CustomerId = corporateCustomer.Id
            };
            
            await _userService.AddAsync(user);
            return new SuccessDataResult<User>(user, Messages.UserRegistered);
        }

        public async Task<IDataResult<User>> LoginAsync(UserForLoginDto userForLoginDto)
        {
            var userToCheck = await _userService.GetByMailAsync(userForLoginDto.Email);
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

        public async Task<IResult> UserExistsAsync(string email)
        {
            if (await _userService.GetByMailAsync(email) != null)
            {
                return new ErrorResult(Messages.UserAlreadyExists);
            }
            return new SuccessResult();
        }

        public async Task<IDataResult<AccessToken>> CreateAccessTokenAsync(User user)
        {
            var claims = await _userService.GetClaimsAsync(user);

            var customerResult = await _customerService.GetByIdAsync(user.CustomerId);
            var customer = customerResult.Data;
            string displayName = "Unknown";

            if (customer is IndividualCustomer individual)
            {
                displayName = $"{individual.FirstName} {individual.LastName}";
            }
            else if (customer is CorporateCustomer corporate)
            {
                displayName = corporate.CompanyName;
            }

            var additionalClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, displayName),
                new Claim("CustomerId", user.CustomerId.ToString())
            };

            var accessToken = _tokenHelper.CreateToken(user, claims, additionalClaims);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }

        public async Task<IDataResult<User>> UpdatePasswordAsync(UserForPasswordDto userForPasswordDto, string newPassword)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(newPassword, out passwordHash, out passwordSalt);
            var result = await _userService.GetByIdAsync(userForPasswordDto.UserId);
            var updatedUser = result.Data;

            if (!HashingHelper.VerifyPasswordHash(userForPasswordDto.OldPassword, updatedUser.PasswordHash, updatedUser.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            if (!userForPasswordDto.NewPassword.Equals(userForPasswordDto.RepeatNewPassword))
            {
                return new ErrorDataResult<User>("Þifre tekrarý yanlýþ!");
            };

            updatedUser.PasswordHash = passwordHash;
            updatedUser.PasswordSalt = passwordSalt;

            await _userService.UpdateAsync(updatedUser);
            return new SuccessDataResult<User>(updatedUser, Messages.UserPasswordUpdated);
        }

        public async Task<IDataResult<User>> RegisterAdminAsync(IndividualRegisterDto registerDto)
        {
            if (await _userService.GetByMailAsync(registerDto.Email) != null)
            {
                return new ErrorDataResult<User>(Messages.UserAlreadyExists);
            }

            var individualCustomer = new IndividualCustomer
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                IdentityNumber = registerDto.IdentityNumber,
                PhoneNumber = registerDto.PhoneNumber,
                CreatedDate = DateTime.Now
            };

            await _individualCustomerService.AddAsync(individualCustomer);

            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);

            var adminUser = new User
            {
                Email = registerDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
                CustomerId = individualCustomer.Id
            };

            await _userService.AddAsync(adminUser);
            return new SuccessDataResult<User>(adminUser, "Admin kullanýcýsý baþarýyla oluþturuldu");
        }
    }
}

