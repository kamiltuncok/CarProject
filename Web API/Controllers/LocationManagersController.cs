using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationManagersController : ControllerBase
    {
        private ILocationManagerUserService _locationManagerUserService;

        public LocationManagersController(ILocationManagerUserService locationManagerUserService)
        {
            _locationManagerUserService = locationManagerUserService;
        }

        [HttpPost]
        [SecuredOperation("admin")]
        public async Task<IActionResult> Add([FromBody] LocationManagerAddDto dto)
        {
            var result = await _locationManagerUserService.AddLocationManagerAsync(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _locationManagerUserService.GetLocationManagersAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut]
        [SecuredOperation("admin")]
        public async Task<IActionResult> Update([FromBody] LocationManagerUpdateDto dto)
        {
            var result = await _locationManagerUserService.UpdateLocationManagerAsync(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("revoke")]
        [SecuredOperation("admin")]
        public async Task<IActionResult> Revoke([FromQuery] int userId, [FromQuery] int locationId)
        {
            var result = await _locationManagerUserService.RevokeLocationManagerAsync(userId, locationId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

