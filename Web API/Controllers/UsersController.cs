using Business.Abstract;
using Core.Entities.Concrete;
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
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        IIndividualCustomerService _individualCustomerService;

        public UsersController(IUserService userService, IIndividualCustomerService individualCustomerService)
        {
            _userService = userService;
            _individualCustomerService = individualCustomerService;
        }

        [HttpGet("getbymail")]
        public IActionResult GetByEmailWithResult(string email)
        {
            var result = _userService.GetByEmailWithResult(email);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int userId)
        {
            var result = _userService.GetById(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update(User user)
        {
            var result = _userService.Update(user);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("updateusernames")]
        public IActionResult UpdateUserNames(UserNamesForUpdateDto userNamesForUpdate)
        {
            var userResult = _userService.GetById(userNamesForUpdate.Id);
            if (!userResult.Success) return BadRequest(userResult);

            var icResult = _individualCustomerService.GetById(userResult.Data.CustomerId);
            if (!icResult.Success) return BadRequest(new Core.Utilities.Results.ErrorResult("Sadece bireysel müşteriler profil ismini güncelleyebilir."));

            var ic = icResult.Data;
            ic.FirstName = userNamesForUpdate.FirstName;
            ic.LastName = userNamesForUpdate.LastName;

            var updateResult = _individualCustomerService.Update(ic);
            if (updateResult.Success)
            {
                return Ok(updateResult);
            }
            return BadRequest(updateResult);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(User user)
        {
            var result = _userService.Delete(user);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
