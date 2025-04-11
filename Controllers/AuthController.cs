using Microsoft.AspNetCore.Mvc;
using best_hackathon_2025.Repositories.Interfaces;
using best_hackathon_2025.MongoDB.Collections;
using best_hackathon_2025.Helpers;

namespace best_hackathon_2025.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
                return BadRequest("User already exists");

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                HashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "user"
            };

            await _userRepository.CreateAsync(user);
            return Ok("Registered successfully!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req,
                                       [FromServices] JwtTokenGenerator jwt)
        {
            var user = await _userRepository.GetByEmailAsync(req.Email);
            if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.HashedPassword))
                return Unauthorized("Wrong credentials");

            var token = jwt.Generate(user);
            Console.WriteLine("SIGNED TOKEN: " + token[..30] + "...");
            return Ok(new { token });
        }

        [HttpGet("generate-token")]
        public IActionResult GenerateToken()
        {
            var token = TokenManager.GenerateToken();
            return Ok(new { token });
        }

        [HttpPost("validate-token")]
        public IActionResult ValidateToken([FromBody] TokenValidationRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                Console.WriteLine("Токен відсутній у запиті.");
                return Unauthorized("Token is missing");
            }

            if (!TokenManager.ValidateToken(request.Token, out var expiry))
            {
                Console.WriteLine($"Токен {request.Token} недійсний або прострочений.");
                return Unauthorized("Invalid or expired token");
            }

            // Видаляємо токен після перевірки
            TokenManager.RemoveToken(request.Token);
            Console.WriteLine($"Токен {request.Token} успішно перевірено.");
            return Ok();
        }

        public record LoginRequest(string Email, string Password);
        public record TokenValidationRequest(string Token);
    }

    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public static string GenerateToken()
        {
            return TokenManager.GenerateToken();
        }
    }
}
