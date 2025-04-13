using best_hackathon_2025.Repositories.Interfaces;
using best_hackathon_2025.MongoDB.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using best_hackathon_2025.Helpers;

namespace best_hackathon_2025.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointController : ControllerBase
    {
        private readonly IPointRepository _pointRepository;
        private readonly LoiCalculator _loiCalculator;

        public PointController(IPointRepository pointRepository)
        {
            _pointRepository = pointRepository;
            _loiCalculator = new LoiCalculator();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var points = await _pointRepository.GetAllAsync();

            foreach (var point in points)
            {
                point.LOI = point.ManualLOI ?? _loiCalculator.CalculateLoi(point);
            }

            return Ok(points);
        }

        [Authorize]
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

        [HttpPost("calculate-loi")]
        public async Task<IActionResult> CalculateLoi([FromBody] CalculateLoiRequest request)
        {
            var points = await _pointRepository.GetAllAsync();

            var result = points.Select(p => new
            {
                p.Id,
                Loi = _loiCalculator.CalculateLoi(p, request.DisabilityType)
            });

            return Ok(result);
        }

        [Authorize(Roles = "admin,inclusive")]
        [HttpPut("updateLoi/{id}")]
        public async Task<IActionResult> UpdateLoi(string id, [FromBody] LoiRequestPoint req)
        {
            var point = await _pointRepository.GetByIdAsync(id);
            if (point == null) return NotFound();

            point.ManualLOI = (int)req.Loi;
            await _pointRepository.UpdateAsync(id, point);
            return Ok("Доступність оновлено!");
        }




        public class LoiRequestPoint
        {
            public int Loi { get; set; } // int бо LOI у тебе int
        }

        public class CalculateLoiRequest { public string? DisabilityType { get; set; } }
    }
}
