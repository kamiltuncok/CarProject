using Business.Abstract;
using Core.Entities.Concrete;
using Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")] // Kullanıcı yönetimi yalnızca admin.
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        private static UserResponseDto ToDto(User user) => user == null ? null : new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            Status = user.Status,
            CustomerId = user.CustomerId
        };

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            // PasswordHash/PasswordSalt sızdırmamak için DTO'ya projekte edilir.
            var dtos = users.Select(ToDto).ToList();
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userService.GetByIdAsync(id);
            if (result.Success && result.Data != null) return Ok(ToDto(result.Data));
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            await _userService.AddAsync(user);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            user.Id = id;
            var result = await _userService.UpdateAsync(user);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userResult = await _userService.GetByIdAsync(id);
            if (!userResult.Success || userResult.Data == null) return BadRequest("Kullanıcı bulunamadı.");

            var result = await _userService.DeleteAsync(userResult.Data);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("update-names")]
        public async Task<IActionResult> UpdateUserNames([FromBody] Entities.DTOs.UserNamesForUpdateDto dto)
        {
            var result = await _userService.UpdateUserNamesAsync(dto);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}
