using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    /// <summary>
    /// Exposes city lookup data.
    /// GET /api/locationcities/getall  →  dropdown list for the frontend
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LocationCitiesController : ControllerBase
    {
        private readonly ILocationCityService _locationCityService;

        public LocationCitiesController(ILocationCityService locationCityService)
        {
            _locationCityService = locationCityService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _locationCityService.GetAll();
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _locationCityService.GetById(id);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(LocationCity locationCity)
        {
            var result = _locationCityService.Add(locationCity);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update(LocationCity locationCity)
        {
            var result = _locationCityService.Update(locationCity);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(LocationCity locationCity)
        {
            var result = _locationCityService.Delete(locationCity);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
