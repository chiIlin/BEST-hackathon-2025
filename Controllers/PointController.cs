using best_hackathon_2025.Repositories.Interfaces;
using best_hackathon_2025.MongoDB.Collections;
using Microsoft.AspNetCore.Mvc;

namespace best_hackathon_2025.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointController : ControllerBase
    {
        private readonly IPointRepository _pointRepository;

        public PointController(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _pointRepository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var point = await _pointRepository.GetByIdAsync(id);
            return point == null ? NotFound("Point not found") : Ok(point);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Point point)
        {
            await _pointRepository.CreateAsync(point);
            return Ok("Point created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Point point)
        {
            var existing = await _pointRepository.GetByIdAsync(id);
            if (existing == null) return NotFound("Point not found");

            point.Id = id;
            await _pointRepository.UpdateAsync(id, point);
            return Ok("Point updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _pointRepository.GetByIdAsync(id);
            if (existing == null) return NotFound("Point not found");

            await _pointRepository.DeleteAsync(id);
            return Ok("Point deleted successfully");
        }
    }

}
