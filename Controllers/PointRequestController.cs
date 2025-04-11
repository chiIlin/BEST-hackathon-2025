// Controllers/PointRequestController.cs
using best_hackathon_2025.MongoDB.Collections;
using best_hackathon_2025.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace best_hackathon_2025.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PointRequestController : ControllerBase
{
    private readonly IPointRequestRepository _reqRepo;
    private readonly IPointRepository _pointRepo;

    public PointRequestController(IPointRequestRepository req, IPointRepository points)
        => (_reqRepo, _pointRepo) = (req, points);

    /* ---------- USER: створити запит ---------- */
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProposedDto dto)
    {
        var req = new PointRequest
        {
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!,
            Proposed = new ProposedPoint
            {
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Address = dto.Address,
                Description = dto.Description,
                Categories = dto.Categories ?? [],
                LOI = dto.LOI
            }
        };
        await _reqRepo.CreateAsync(req);
        return Ok(req);
    }

    /* ---------- ADMIN: список запитів ---------- */
    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> All() => Ok(await _reqRepo.GetAllAsync());

    /* ---------- ADMIN: схвалити ---------- */
    [Authorize(Roles = "admin")]
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(string id)
    {
        var req = await _reqRepo.GetByIdAsync(id);
        if (req is null) return NotFound();

        var p = new Point
        {
            Latitude = req.Proposed.Latitude,
            Longitude = req.Proposed.Longitude,
            Address = req.Proposed.Address,
            Description = req.Proposed.Description,
            Categories = req.Proposed.Categories,
            LOI = req.Proposed.LOI,
            Rating = 0,
            Verified = true
        };
        await _pointRepo.CreateAsync(p);
        await _reqRepo.DeleteAsync(id);
        return Ok(p);
    }

    public record ProposedDto(
        double Latitude, double Longitude,
        string Address, string Description,
        List<string>? Categories, int LOI);
}
