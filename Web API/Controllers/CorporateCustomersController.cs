using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorporateCustomersController : ControllerBase
    {
        private readonly ICorporateCustomerService _service;

        public CorporateCustomersController(ICorporateCustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CorporateCustomer entity)
        {
            var result = await _service.AddAsync(entity);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CorporateCustomer entity)
        {
            entity.Id = id;
            var result = await _service.UpdateAsync(entity);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entityResult = await _service.GetByIdAsync(id);
            if (!entityResult.Success || entityResult.Data == null) return BadRequest("Kurumsal müşteri bulunamadı.");
            
            var result = await _service.DeleteAsync(entityResult.Data);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}
