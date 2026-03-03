using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System.Net.Http;
using System.Text;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        ICarService _carService;
        IPricingService _pricingService;
        public CarsController(ICarService carService,IPricingService pricingService)
        {
            _carService = carService;
            _pricingService = pricingService;
        }

        // GET api/cars
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _carService.GetAllAsync();
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
        
        // GET api/cars/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _carService.GetByIdAsync(id);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        // POST api/cars
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CarCreateDto carDto)
        {
            var result = await _carService.AddAsync(carDto);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        // PUT api/cars/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CarUpdateDto carDto)
        {
            carDto.Id = id; // Ensure ID matches URL
            var result = await _carService.UpdateAsync(carDto);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        // DELETE api/cars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _carService.DeleteAsync(id);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        // --- Detail Projection Endpoints ---
        
        // GET api/cars/details
        [HttpGet("details")]
        public async Task<IActionResult> GetCarDetails()
        {
            var result = await _carService.GetCarDetailDtosAsync();
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        // GET api/cars/details/5
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetCarDetailsById(int id)
        {
            var result = await _carService.GetCarDetailsByIdAsync(id);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        // GET api/cars/brand/5
        [HttpGet("brand/{brandId}")]
        public async Task<IActionResult> GetByBrands(int brandId)
        {
            var result = await _carService.GetCarDetailsByBrandIdAsync(brandId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        // GET api/cars/color/5
        [HttpGet("color/{colorId}")]
        public async Task<IActionResult> GetByColors(int colorId)
        {
            var result = await _carService.GetCarDetailsByColorIdAsync(colorId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        // POST api/cars/available
        [HttpPost("available")]
        public async Task<IActionResult> GetAvailableCars([FromBody] CarAvailabilityFilterDto filter)
        {
            var result = await _carService.GetAvailableCarsAsync(filter);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        // --- Pricing Endpoints ---

        // GET api/cars/5/recommended-price
        [HttpGet("{carId}/recommended-price")]
        public async Task<IActionResult> GetRecommendedPrice(int carId)
        {
            try
            {
                var action = await _pricingService.GetRecommendedActionAsync(carId);
                return Ok(new { CarId = carId, RecommendedAction = action });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        // POST api/cars/5/update-price
        [HttpPost("{carId}/update-price")]
        public async Task<IActionResult> UpdatePrice(int carId)
        {
            string action = await _pricingService.GetRecommendedActionAsync(carId);
            var result = await _carService.UpdatePriceByActionAsync(carId, action);

            if (result.Success) return Ok(new { CarId = carId, NewPrice = result.Data, Action = action });
            return BadRequest(result.Message);
        }

        // POST api/cars/update-prices-batch
        [HttpPost("update-prices-batch")]
        public async Task<IActionResult> UpdateAllPrices()
        {
            try
            {
                var result = await _pricingService.UpdateAllPricesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Fiyat güncelleme sırasında bir hata oluştu.", details = ex.Message });
            }
        }

        // GET api/cars/segment/5/lowest-price
        [HttpGet("segment/{segmentId}/lowest-price")]
        public async Task<IActionResult> GetLowestPriceBySegment(int segmentId, [FromQuery] bool isRented = false)
        {
            var result = await _carService.GetLowestPriceBySegmentIdAsync(segmentId, isRented);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}
