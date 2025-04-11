using best_hackathon_2025.Repositories.Interfaces;
using best_hackathon_2025.MongoDB.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize]                     // лише залогінені
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PointDto dto)
        {
            var point = new Point
            {
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Address = dto.Address,
                Description = dto.Description,
                Categories = dto.Categories ?? [],
                LOI = dto.LOI,
                Rating = 0,
                Verified = false
            };
            await _pointRepository.CreateAsync(point);
            return Ok(point);
        }

        public record PointDto(
            double Latitude,
            double Longitude,
            string Address,
            string Description,
            List<string> Categories,
            int LOI);

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var point = await _pointRepository.GetByIdAsync(id);
            return point == null ? NotFound("Point not found") : Ok(point);
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
