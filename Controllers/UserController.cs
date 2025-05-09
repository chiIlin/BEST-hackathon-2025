﻿using Microsoft.AspNetCore.Mvc;
using best_hackathon_2025.Repositories.Interfaces;
using best_hackathon_2025.MongoDB.Collections;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace best_hackathon_2025.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _userRepository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? NotFound("User not found") : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            await _userRepository.CreateAsync(user);
            return Ok("User created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] User user)
        {
            var existing = await _userRepository.GetByIdAsync(id);
            if (existing == null) return NotFound("User not found");

            user.Id = id;
            await _userRepository.UpdateAsync(id, user);
            return Ok("User updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _userRepository.GetByIdAsync(id);
            if (existing == null) return NotFound("User not found");

            await _userRepository.DeleteAsync(id);
            return Ok("User deleted successfully");
        }

        [Authorize]
        [HttpPut("me")]
        public async Task<IActionResult> UpdateMe([FromBody] User user)
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != user.Id)
                return BadRequest("Invalid user");

            await _userRepository.UpdateAsync(id, user);
            return Ok();
        }




        // Controllers/UserController.cs  (додайте всередині вже існуючого класу)
        [Authorize]                     // токен обов’язковий
        [HttpGet("me")]
        public async Task<IActionResult> Me([FromServices] IUserRepository users)
        {
            Console.WriteLine("ME ENDPOINT CALLED");
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id is null) return Unauthorized();

            var user = await users.GetByIdAsync(id);
            return user is null ? NotFound() : Ok(user);
        }

        [Authorize]
        [HttpPost("savePoint/{pointId}")]
        public async Task<IActionResult> SavePoint(string pointId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            await _userRepository.SavePointAsync(userId, pointId);
            return Ok();
        }

        [HttpPost("removePoint/{pointId}")]
        [Authorize]
        public async Task<IActionResult> RemovePoint(string pointId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            await _userRepository.RemovePointAsync(userId, pointId);
            return Ok();
        }

        [Authorize]
        [HttpDelete("removeSavedPoint/{pointId}")]
        public async Task<IActionResult> RemoveSavedPoint(string pointId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            await _userRepository.RemoveSavedPointAsync(userId, pointId);
            return Ok();
        }


        [HttpPost("byIds")]
        public async Task<IActionResult> GetUsersByIds([FromBody] List<string> ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest("Список id пустий");

            var users = await _userRepository.GetManyByIdsAsync(ids);

            var result = users.Select(u => new
            {
                id = u.Id,
                name = u.Name
            });

            return Ok(result);
        }

    }
}
