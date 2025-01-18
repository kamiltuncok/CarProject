using Business.Abstract;
using Core.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorporateUsersController : ControllerBase
    {
        ICorporateUserService _corporateUserService;
        public CorporateUsersController(ICorporateUserService corporateUserService)
        {
            _corporateUserService = corporateUserService;
        }

        [HttpGet("getbymail")]
        public IActionResult GetByEmailWithResult(string email)
        {
            var result = _corporateUserService.GetByEmailWithResult(email);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int userId)
        {
            var result = _corporateUserService.GetById(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update(CorporateUser corporateUser)
        {
            var result = _corporateUserService.Update(corporateUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpDelete("delete")]
        public IActionResult Delete(CorporateUser corporateUser)
        {
            var result = _corporateUserService.Delete(corporateUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
