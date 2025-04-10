using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using best_hackathon_2025.Repositories.Interfaces;
using best_hackathon_2025.MongoDB.Collections;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace BEST_hackathon_2025.Pages.Auth;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly IUserRepository _users;
    public ProfileModel(IUserRepository users) => _users = users;

    public User? ProfileUser { get; private set; }   // ← перейменовано

    public async Task OnGetAsync()
    {
        var id = HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (id is null) return;

        ProfileUser = await _users.GetByIdAsync(id);
    }
}
