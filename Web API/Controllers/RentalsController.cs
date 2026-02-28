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
        IRentalService _rentalService;
        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _rentalService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("getbyid")]
        public IActionResult GetById(int rentalid)
        {
            var result = _rentalService.GetById(rentalid);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("update")]
        public IActionResult Update(Rental rental)
        {
            var result = _rentalService.Update(rental);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("delete")]
        public IActionResult Delete(Rental rental)
        {
            var result = _rentalService.Delete(rental);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("add")] // Keeping old Add for whatever reason, probably unused but safe
        public IActionResult Post(Rental rental)
        {
            var result = _rentalService.Add(rental);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("create")]
        public IActionResult CreateRental([FromBody] Entities.DTOs.RentalCreateRequestDto request)
        {
            // Extract the user ID from the JWT token.
            // Assuming default ASP.NET Core Identity/JWT claims handling where NameIdentifier holds User ID.
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { Message = "Kuallnıcı kimliği doğrulanamadı (Token eksik veya geçersiz)." });
            }

            // Route to the new strict domain Service
            var result = _rentalService.CreateRental(request, userId);
            
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("createguest")]
        public IActionResult CreateGuestRental([FromBody] Entities.DTOs.GuestRentalCreateRequestDto request)
        {
            var result = _rentalService.CreateGuestRental(request);
            
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpGet("getrentalsbyuserid")]
        public IActionResult GetRentalsByUserId(int userId,CustomerType customerType)
        {
            var result = _rentalService.GetRentalDetailsByUserId(userId, customerType);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add-bulk")]
        public IActionResult AddBulk([FromBody] List<Rental> rentals)
        {
            if (rentals == null || !rentals.Any())  // HATA BURADA
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Rental listesi boş"
                });
            }

            var result = _rentalService.AddBulk(rentals);

            if (result.Success)
            {
                return Ok(new
                {
                    success = true,
                    count = rentals.Count,
                    message = result.Message
                });
            }

            return BadRequest(new
            {
                success = false,
                message = result.Message
            });
        }

        [HttpGet("getbystartdate")]
        public IActionResult GetRentalsByStartDate(DateTime startDate)
        {
            var result = _rentalService.GetRentalsByStartDate(startDate);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyemail")]
        public IActionResult GetRentalsByEmail(string email)
        {
            var result = _rentalService.GetRentalsByEmail(email);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyname")]
        public IActionResult GetRentalsByName(string name)
        {
            var result = _rentalService.GetRentalsByName(name);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbydaterange")]
        public IActionResult GetRentalsByDateRange(DateTime startDate, DateTime endDate)
        {
            var result = _rentalService.GetRentalsByDateRange(startDate, endDate);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("markasreturned")]
        public IActionResult MarkAsReturned(int rentalId)
        {
            var result = _rentalService.MarkAsReturned(rentalId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("deleteandfreecardendpoint")]
        public IActionResult DeleteAndFreeCar(int rentalId)
        {
            var result = _rentalService.DeleteAndFreeCar(rentalId);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
