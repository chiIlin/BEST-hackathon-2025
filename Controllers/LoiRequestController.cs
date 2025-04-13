using best_hackathon_2025.MongoDB.Collections;
using best_hackathon_2025.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using best_hackathon_2025.MongoDB;

using System.Security.Claims;

namespace best_hackathon_2025.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoiRequestController : ControllerBase
    {
        private readonly ILoiRequestRepository _loiRequestRepository;
        private readonly IMongoCollection<Point> _pointCollection;

        public LoiRequestController(ILoiRequestRepository loiRequestRepository, MongoDbContext context)
        {
            _loiRequestRepository = loiRequestRepository;
            _pointCollection = context.Points;  // Додаєш це!!!
        }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _loiRequestRepository.GetAllAsync();
            return Ok(requests);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetById(string id)
        {
            var request = await _loiRequestRepository.GetByIdAsync(id);
            return request == null ? NotFound("Request not found") : Ok(request);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] LoiRequestDto dto)
        {
            var request = new LoiRequest
            {
                PointId = dto.PointId,
                RequestedLoi = dto.RequestedLoi,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                TimeCreated = DateTime.UtcNow,
                Status = "pending"
            };

            await _loiRequestRepository.CreateAsync(request);
            return Ok("Request created");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(string id, [FromBody] LoiRequest request)
        {
            var existing = await _loiRequestRepository.GetByIdAsync(id);
            if (existing == null) return NotFound("Request not found");

            request.Id = id;
            await _loiRequestRepository.UpdateAsync(id, request);
            return Ok("Request updated");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _loiRequestRepository.GetByIdAsync(id);
            if (existing == null) return NotFound("Request not found");

            await _loiRequestRepository.DeleteAsync(id);
            return Ok("Request deleted");
        }

        [HttpPost("{id}/approve")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Approve(string id)
        {
            var request = await _loiRequestRepository.GetByIdAsync(id);
            if (request == null)
                return NotFound("Request not found");

            request.Status = "approved";
            await _loiRequestRepository.UpdateAsync(id, request);

            await _loiRequestRepository.UpdatePointManualLoiAsync(request.PointId, request.RequestedLoi);


            return Ok("LOI updated");
        }




    }


    public class LoiRequestDto
    {
        public string PointId { get; set; } = string.Empty;
        public int RequestedLoi { get; set; }
    }
}
