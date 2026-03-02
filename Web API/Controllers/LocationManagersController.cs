using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Entities.DTOs;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("add")]
        [SecuredOperation("admin")]
        public IActionResult Add(LocationManagerAddDto dto)
        {
            var result = _locationManagerUserService.AddLocationManager(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _locationManagerUserService.GetLocationManagers();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        [SecuredOperation("admin")]
        public IActionResult Update(LocationManagerUpdateDto dto)
        {
            var result = _locationManagerUserService.UpdateLocationManager(dto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("revoke")]
        [SecuredOperation("admin")]
        public IActionResult Revoke([FromQuery] int userId, [FromQuery] int locationId)
        {
            var result = _locationManagerUserService.RevokeLocationManager(userId, locationId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
