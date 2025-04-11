using best_hackathon_2025.MongoDB.Collections;
using best_hackathon_2025.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Concurrent;

public class RegisterModel : PageModel
{
    private readonly IUserRepository _userRepository;

    // Сховище для тимчасових токенів (можна замінити на базу даних)
    private static readonly ConcurrentDictionary<string, DateTime> TokenStore = new();

    public RegisterModel(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [BindProperty]
    public RegisterInput Input { get; set; }

    public string Message { get; set; }

    public class RegisterInput
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public IActionResult OnGet()
    {
        // Більше не перевіряємо токен тут
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Message = "Invalid input";
            return Page();
        }

        var existingUser = await _userRepository.GetByEmailAsync(Input.Email);
        if (existingUser != null)
        {
            Message = "User already exists";
            return Page();
        }

        var user = new User
        {
            Name = Input.Name,
            Email = Input.Email,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(Input.Password),
            Role = "user"
        };

        await _userRepository.CreateAsync(user);
        Message = "Registration successful!";
        return RedirectToPage("/Auth/Login");
    }

    // Метод для генерації токена
    public static string GenerateToken()
    {
        var token = Guid.NewGuid().ToString();
        TokenStore[token] = DateTime.UtcNow.AddMinutes(10); // Токен дійсний 10 хвилин
        return token;
    }
}
