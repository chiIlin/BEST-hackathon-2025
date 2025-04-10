using Microsoft.AspNetCore.Mvc;
using best_hackathon_2025.Repositories.Interfaces;
using best_hackathon_2025.MongoDB.Collections;

namespace best_hackathon_2025.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointRequestController : ControllerBase
    {
        private readonly IPointRequestRepository _pointRequestRepository;

        public PointRequestController(IPointRequestRepository pointRequestRepository)
        {
            _pointRequestRepository = pointRequestRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _pointRequestRepository.GetAllAsync();
            return Ok(requests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var request = await _pointRequestRepository.GetByIdAsync(id);
            if (request == null)
                return NotFound("PointRequest not found");

            return Ok(request);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PointRequest request)
        {
            await _pointRequestRepository.CreateAsync(request);
            return Ok("PointRequest created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] PointRequest request)
        {
            var existing = await _pointRequestRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound("PointRequest not found");

            request.Id = id; // important for ReplaceOneAsync
            await _pointRequestRepository.UpdateAsync(id, request);
            return Ok("PointRequest updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _pointRequestRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound("PointRequest not found");

            await _pointRequestRepository.DeleteAsync(id);
            return Ok("PointRequest deleted successfully");
        }
    }
}
