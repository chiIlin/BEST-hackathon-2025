using best_hackathon_2025.MongoDB.Collections;
using best_hackathon_2025.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class RegisterModel : PageModel
{
    private readonly IUserRepository _userRepository;

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

    public async Task<IActionResult> OnPostAsync()
    {
        Console.WriteLine("Register Attempt");

        if (!ModelState.IsValid)
        {
            Console.WriteLine("Model is invalid");
            return Page();
        }

        Console.WriteLine($"Trying to register user: {Input.Email}");

        var existingUser = await _userRepository.GetByEmailAsync(Input.Email);
        if (existingUser != null)
        {
            Console.WriteLine("User already exists in DB");
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

        Console.WriteLine("Creating user in DB...");
        await _userRepository.CreateAsync(user);
        Console.WriteLine("User created successfully!");
  
        Message = "Registration successful!";
        return RedirectToPage("/Auth/Login");
    }

}
