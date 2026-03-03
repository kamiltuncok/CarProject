using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = await _authService.LoginAsync(userForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }

            var result = await _authService.CreateAccessTokenAsync(userToLogin.Data);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("loginforcorporate")]
        public async Task<ActionResult> LoginForCorporateUser(UserForLoginDto userForLoginDto)
        {
            // Corporate login is now exactly the same as Individual login (polymorphic JWT generation)
            return await Login(userForLoginDto);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(IndividualRegisterDto userForRegisterDto)
        {
            var userExists = await _authService.UserExistsAsync(userForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = await _authService.RegisterIndividualAsync(userForRegisterDto);
            if (!registerResult.Success)
            {
                return BadRequest(registerResult.Message);
            }

            var result = await _authService.CreateAccessTokenAsync(registerResult.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("registerforcorporate")]
        public async Task<ActionResult> RegisterForCorporate(CorporateRegisterDto userForRegisterDto)
        {
            var userExists = await _authService.UserExistsAsync(userForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = await _authService.RegisterCorporateAsync(userForRegisterDto);
            if (!registerResult.Success)
            {
                return BadRequest(registerResult.Message);
            }

            var result = await _authService.CreateAccessTokenAsync(registerResult.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("registeradmin")]
        public async Task<ActionResult> RegisterAdmin(IndividualRegisterDto userForRegisterDto)
        {
            var userExists = await _authService.UserExistsAsync(userForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }

            var registerResult = await _authService.RegisterAdminAsync(userForRegisterDto);
            if (!registerResult.Success)
            {
                return BadRequest(registerResult.Message);
            }

            var result = await _authService.CreateAccessTokenAsync(registerResult.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        [HttpPut("password")]
        public async Task<ActionResult> UpdatePassword(UserForPasswordDto userForPasswordDto)
        {
            var result = await _authService.UpdatePasswordAsync(userForPasswordDto, userForPasswordDto.NewPassword);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }
    }
}

