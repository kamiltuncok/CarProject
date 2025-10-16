using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
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

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _carService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        
        [HttpPost("add")]
        public IActionResult Post(Car car)
        {
            var result = _carService.Add(car);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("getbybrandid")]
        public IActionResult GetByBrandId(int brandid)
        {
            var result = _carService.GetCarsByBrandId(brandid);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("getbycolorid")]
        public IActionResult GetByColorId(int colorid)
        {
            var result = _carService.GetCarsByBrandId(colorid);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("update")]
        public IActionResult Update(Car car)
        {
            var result = _carService.Update(car);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("delete")]
        public IActionResult Delete(Car car)
        {
            var result = _carService.Delete(car);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("transaction")]
        public IActionResult TransactionTest(Car car)
        {
            var result = _carService.AddTransactionalTest(car);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
        [HttpGet("getcardetails")]
        public IActionResult GetCarDetails()
        {
            var result = _carService.GetCarDetails();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("getbybrands")]
        public IActionResult GetByBrands(int brandId)
        {
            var result = _carService.GetCarDetailsByBrandId(brandId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        [HttpGet("getbycolors")]
        public IActionResult GetByColors(int colorId)
        {
            var result = _carService.GetCarDetailsByColorId(colorId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getbycarid")]
        public IActionResult GetByCarId(int carId)
        {
            var result = _carService.GetCarDetailsById(carId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult Get(int id)
        {
            var result = _carService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("getnotrentedcarsbylocations")]
        public IActionResult GetNotRentedCarsByLocations(int locationId)
        {
            var result = _carService.GetNotRentedCarsByLocationId(locationId,false);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpGet("getcarsnotrentedbylocations")]
        public IActionResult GetCarsNotRentedByLocations(string locationName)
        {
            var result = _carService.GetCarsNotRentedByLocationName(locationName, false);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("carisrented")]
        public void CarRented(int carId) => _carService.CarRented(carId);


        [HttpGet("getcarsbygearandfuelfilters")]
        public IActionResult GetCarsByFilters([FromQuery] List<int> fuelIds, [FromQuery] List<int> gearIds, string locationName)
        {
            var result = _carService.GetCarsByGearAndFuelFilters(fuelIds, gearIds, false, locationName);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("getrecommendedprice")]
        public async Task<IActionResult> GetRecommendedPrice(int carId)
        {
            try
            {
                var action = await _pricingService.GetRecommendedActionAsync(carId);
                return Ok(new
                {
                    CarId = carId,
                    RecommendedAction = action
                });
            }
            catch (Exception ex)
            {
                // Detaylı hatayı JSON olarak döndür
                return StatusCode(500, new
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }


        [HttpPost("updateprice")]
        public async Task<IActionResult> UpdatePrice(int carId)
        {
            string action = await _pricingService.GetRecommendedActionAsync(carId);
            var result = _carService.UpdatePriceByAction(carId, action);

            if (result.Success)
                return Ok(new { CarId = carId, NewPrice = result.Data, Action = action });

            return BadRequest(result.Message);
        }


        [HttpPost("update-prices")]
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




    }
}
