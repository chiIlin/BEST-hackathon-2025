using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using best_hackathon_2025.Repositories.Interfaces;

public class LoginModel : PageModel
{
    private readonly IUserRepository _userRepository;

    public LoginModel(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [BindProperty]
    public LoginInput Input { get; set; }

    public string Message { get; set; }

    public class LoginInput
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userRepository.GetByEmailAsync(Input.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(Input.Password, user.HashedPassword))
        {
            Message = "Invalid credentials";
            return Page();
        }

        Message = "Login successful!";
        return RedirectToPage("/Index");
    }
}
