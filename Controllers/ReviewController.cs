using Microsoft.AspNetCore.Mvc;
using best_hackathon_2025.Repositories.Interfaces;
using best_hackathon_2025.MongoDB.Collections;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReviewRequest request, [FromServices] IPointRepository points)
        {
            var review = new Review
            {
                PointId = request.PointId,
                ReviewText = request.ReviewText,
                Rating = request.Rating,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                TimeCreated = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };

            await _reviewRepository.CreateAsync(review);

            // Додаємо Review.Id у Point
            await points.AddReviewToPointAsync(review.PointId, review.Id);

            return Ok("Review created");
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

        [HttpGet("byPoint/{pointId}")]
        public async Task<IActionResult> GetByPoint(string pointId)
        {
            var reviews = await _reviewRepository.GetAllAsync();
            var filtered = reviews.Where(x => x.PointId == pointId).ToList();

            if (filtered.Count == 0)
                return NotFound();

            return Ok(filtered);
        }

    }
}
