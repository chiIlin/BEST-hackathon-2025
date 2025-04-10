using Microsoft.AspNetCore.Mvc;
using best_hackathon_2025.Repositories.Interfaces;
using best_hackathon_2025.MongoDB.Collections;

namespace best_hackathon_2025.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _reviewRepository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            return review == null ? NotFound("Review not found") : Ok(review);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Review review)
        {
            await _reviewRepository.CreateAsync(review);
            return Ok("Review created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Review review)
        {
            var existing = await _reviewRepository.GetByIdAsync(id);
            if (existing == null) return NotFound("Review not found");

            review.Id = id;
            await _reviewRepository.UpdateAsync(id, review);
            return Ok("Review updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _reviewRepository.GetByIdAsync(id);
            if (existing == null) return NotFound("Review not found");

            await _reviewRepository.DeleteAsync(id);
            return Ok("Review deleted successfully");
        }
    }
}
