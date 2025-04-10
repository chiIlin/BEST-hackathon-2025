using Microsoft.AspNetCore.Mvc;
using best_hackathon_2025.Repositories.Interfaces;
using best_hackathon_2025.MongoDB.Collections;

namespace best_hackathon_2025.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransportController : ControllerBase
    {
        private readonly ITransportRepository _transportRepository;

        public TransportController(ITransportRepository transportRepository)
        {
            _transportRepository = transportRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _transportRepository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var transport = await _transportRepository.GetByIdAsync(id);
            return transport == null ? NotFound("Transport not found") : Ok(transport);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Transport transport)
        {
            await _transportRepository.CreateAsync(transport);
            return Ok("Transport created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Transport transport)
        {
            var existing = await _transportRepository.GetByIdAsync(id);
            if (existing == null) return NotFound("Transport not found");

            transport.Id = id;
            await _transportRepository.UpdateAsync(id, transport);
            return Ok("Transport updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _transportRepository.GetByIdAsync(id);
            if (existing == null) return NotFound("Transport not found");

            await _transportRepository.DeleteAsync(id);
            return Ok("Transport deleted successfully");
        }
    }
}
