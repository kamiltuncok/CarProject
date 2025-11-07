using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationOperationClaimsController : ControllerBase
    {
        ILocationOperationClaimService _locationOperationClaimService;

        public LocationOperationClaimsController(ILocationOperationClaimService locationOperationClaimService)
        {
            _locationOperationClaimService = locationOperationClaimService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _locationOperationClaimService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int locationOperationClaimId)
        {
            var result = _locationOperationClaimService.GetById(locationOperationClaimId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyuserid")]
        public IActionResult GetByUserId(int userId)
        {
            var result = _locationOperationClaimService.GetByUserId(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbylocationid")]
        public IActionResult GetByLocationId(int locationId)
        {
            var result = _locationOperationClaimService.GetByLocationId(locationId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyuserandlocation")]
        public IActionResult GetByUserAndLocation(int userId, int locationId)
        {
            var result = _locationOperationClaimService.GetByUserAndLocation(userId, locationId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("isuserlocationmanager")]
        public IActionResult IsUserLocationManager(int userId, int locationId)
        {
            var result = _locationOperationClaimService.IsUserLocationManager(userId, locationId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getrentalsbymanagerlocation")]
        public IActionResult GetRentalsByManagerLocation(int userId)
        {
            var result = _locationOperationClaimService.GetRentalsByManagerLocation(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getlistbyid")]
        public IActionResult GetListById(int id)
        {
            var result = _locationOperationClaimService.GetListById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update(LocationOperationClaim locationOperationClaim)
        {
            var result = _locationOperationClaimService.Update(locationOperationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(LocationOperationClaim locationOperationClaim)
        {
            var result = _locationOperationClaimService.Delete(locationOperationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Post(LocationOperationClaim locationOperationClaim)
        {
            var result = _locationOperationClaimService.Add(locationOperationClaim);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}