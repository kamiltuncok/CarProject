using Business.Abstract;
using Entities.Concrete;
using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
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
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _rentalService.GetAllAsync();
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _rentalService.GetByIdAsync(id);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Rental rental)
        {
            var result = await _rentalService.AddAsync(rental);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Rental rental)
        {
            rental.Id = id;
            var result = await _rentalService.UpdateAsync(rental);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rentalResult = await _rentalService.GetByIdAsync(id);
            if (!rentalResult.Success || rentalResult.Data == null) 
                return BadRequest("Kiralama bulunamadı.");

            var result = await _rentalService.DeleteAsync(rentalResult.Data);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRental([FromBody] Entities.DTOs.RentalCreateRequestDto request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { Message = "Kullanıcı kimliği doğrulanamadı (Token eksik veya geçersiz)." });
            }

            var result = _rentalService.CreateRental(request, userId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("createguest")]
        public async Task<IActionResult> CreateGuestRental([FromBody] Entities.DTOs.GuestRentalCreateRequestDto request)
        {
            var result = _rentalService.CreateGuestRental(request);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRentalsByUserId(int userId)
        {
            var result = _rentalService.GetRentalDetailsByUserId(userId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("manager/{userId}")]
        public async Task<IActionResult> GetRentalsByManagerLocation(int userId)
        {
            var result = _rentalService.GetRentalsByManagerLocation(userId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddBulk([FromBody] List<Rental> rentals)
        {
            if (rentals == null || !rentals.Any())
            {
                return BadRequest(new { success = false, message = "Rental listesi boş" });
            }

            var result = _rentalService.AddBulk(rentals);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("date-range")]
        public async Task<IActionResult> GetRentalsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = _rentalService.GetRentalsByDateRange(startDate, endDate);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("{id}/mark-as-returned")]
        public async Task<IActionResult> MarkAsReturned(int id)
        {
            var result = _rentalService.MarkAsReturned(id);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("{id}/collect-deposit")]
        public async Task<IActionResult> CollectDeposit(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized(new { Message = "Kullanıcı kimliği doğrulanamadı." });

            var result = _rentalService.CollectDeposit(id, userId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("{id}/deliver")]
        public async Task<IActionResult> DeliverVehicle(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized(new { Message = "Kullanıcı kimliği doğrulanamadı." });

            var result = _rentalService.DeliverVehicle(id, userId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelRental(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized(new { Message = "Kullanıcı kimliği doğrulanamadı." });

            var result = _rentalService.CancelRental(id, userId);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}

