const fs = require('fs');
const path = require('path');

const basePath = `C:\\Users\\MONSTER\\OneDrive\\Belgeler\\GitHub\\CarProject`;

const entities = [
    { name: 'Brand', namePlural: 'Brands', pk: 'BrandId' },
    { name: 'Color', namePlural: 'Colors', pk: 'ColorId' },
    { name: 'Fuel', namePlural: 'Fuels', pk: 'FuelId' },
    { name: 'Gear', namePlural: 'Gears', pk: 'GearId' },
    { name: 'Segment', namePlural: 'Segments', pk: 'SegmentId' },
    { name: 'Location', namePlural: 'Locations', pk: 'Id' },
    { name: 'LocationCity', namePlural: 'LocationCities', pk: 'Id' }
];

entities.forEach(entity => {
    // 1. IService
    const iServicePath = path.join(basePath, `Business\\Abstract\\I${entity.name}Service.cs`);
    if (fs.existsSync(iServicePath)) {
        let content = fs.readFileSync(iServicePath, 'utf8');
        if (!content.includes('using System.Threading.Tasks;')) {
            content = content.replace('using System.Collections.Generic;', 'using System.Collections.Generic;\r\nusing System.Threading.Tasks;');
        }

        let newMethods = `
        Task<IDataResult<List<${entity.name}>>> GetAllAsync();
        Task<IDataResult<${entity.name}>> GetByIdAsync(int id);
        Task<IResult> AddAsync(${entity.name} entity);
        Task<IResult> UpdateAsync(${entity.name} entity);
        Task<IResult> DeleteAsync(int id);
`;
        if (!content.includes('GetAllAsync')) {
            content = content.replace('}', newMethods + '    }\r\n');
        }
        fs.writeFileSync(iServicePath, content);
    }

    // 2. Manager
    const managerPath = path.join(basePath, `Business\\Concrete\\${entity.name}Manager.cs`);
    if (fs.existsSync(managerPath)) {
        let content = fs.readFileSync(managerPath, 'utf8');
        if (!content.includes('using System.Threading.Tasks;')) {
            content = content.replace('using System.Collections.Generic;', 'using System.Collections.Generic;\r\nusing System.Threading.Tasks;');
        }

        let newMethods = `
        public async Task<IDataResult<List<${entity.name}>>> GetAllAsync()
        {
            return new SuccessDataResult<List<${entity.name}>>(await _${entity.name.toLowerCase()}Dal.GetAllAsync(), Messages.${entity.name}Listed);
        }

        public async Task<IDataResult<${entity.name}>> GetByIdAsync(int id)
        {
            return new SuccessDataResult<${entity.name}>(await _${entity.name.toLowerCase()}Dal.GetAsync(e => e.${entity.pk} == id));
        }

        public async Task<IResult> AddAsync(${entity.name} entity)
        {
            // Omit fluent validation here for brevity unless needed, or just call normal pipeline
            await _${entity.name.toLowerCase()}Dal.AddAsync(entity);
            return new SuccessResult(Messages.${entity.name}Added);
        }

        public async Task<IResult> UpdateAsync(${entity.name} entity)
        {
            await _${entity.name.toLowerCase()}Dal.UpdateAsync(entity);
            return new SuccessResult(Messages.${entity.name}Updated);
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var entity = await _${entity.name.toLowerCase()}Dal.GetAsync(e => e.${entity.pk} == id);
            if (entity != null)
            {
                await _${entity.name.toLowerCase()}Dal.DeleteAsync(entity);
            }
            return new SuccessResult(Messages.${entity.name}Deleted);
        }
`;
        if (!content.includes('GetAllAsync')) {
            content = content.substring(0, content.lastIndexOf('}'));
            content = content.substring(0, content.lastIndexOf('}'));
            content += newMethods + '    }\r\n}\r\n';
        }
        fs.writeFileSync(managerPath, content);
    }

    // 3. Controller
    const controllerPath = path.join(basePath, `Web API\\Controllers\\${entity.namePlural}Controller.cs`);
    if (fs.existsSync(controllerPath)) {
        let content = fs.readFileSync(controllerPath, 'utf8');

        let newController = `using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ${entity.namePlural}Controller : ControllerBase
    {
        private readonly I${entity.name}Service _service;

        public ${entity.namePlural}Controller(I${entity.name}Service service)
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
        public async Task<IActionResult> Add([FromBody] ${entity.name} entity)
        {
            var result = await _service.AddAsync(entity);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ${entity.name} entity)
        {
            entity.${entity.pk} = id;
            var result = await _service.UpdateAsync(entity);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result.Success) return Ok(result);
            return BadRequest(result);
        }
    }
}
`;
        fs.writeFileSync(controllerPath, newController);
    }
});

console.log('Script completed.');
