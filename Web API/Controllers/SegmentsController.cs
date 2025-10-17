using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegmentsController : ControllerBase
    {
        ISegmentService _segmentService;

        public SegmentsController(ISegmentService segmentService)
        {
            _segmentService = segmentService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _segmentService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int segmentId)
        {
            var result = _segmentService.GetById(segmentId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public IActionResult Update(Segment segment)
        {
            var result = _segmentService.Update(segment);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(Segment segment)
        {
            var result = _segmentService.Delete(segment);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(Segment segment)
        {
            var result = _segmentService.Add(segment);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
