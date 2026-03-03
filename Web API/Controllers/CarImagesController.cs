using Business.Abstract;
using Entities.Concrete;
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
    public class CarImagesController : ControllerBase
    {
        ICarImageService _carImageService;
        public CarImagesController(ICarImageService carImageService)
        {
            _carImageService = carImageService;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] IFormFile file, [FromForm] CarImage carImage)
        {
            var result = await _carImageService.AddAsync(file, carImage);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromForm(Name = ("Image"))] IFormFile file, [FromForm(Name = ("Id"))] int id)
        {
            var carImage = (await _carImageService.GetByIdAsync(id)).Data;
            var result = await _carImageService.UpdateAsync(carImage, file);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(CarImage carImage)
        {
            var result = await _carImageService.DeleteAsync(carImage);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _carImageService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("{carId}")]
        public async Task<IActionResult> GetById(int carId)
        {
            var result = await _carImageService.GetByIdAsync(carId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("brand/{brandId}/color/{colorId}")]
        public async Task<IActionResult> GetCarImageByColorAndBrandId(int brandId, int colorId)
        {
            var result = await _carImageService.GetCarImageByColorAndBrandIdAsync(brandId, colorId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}

