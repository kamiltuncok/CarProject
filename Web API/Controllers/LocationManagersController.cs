using Business.Abstract;
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
