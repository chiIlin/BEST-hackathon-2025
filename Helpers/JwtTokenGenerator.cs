using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using best_hackathon_2025.MongoDB.Collections;
using best_hackathon_2025.Settings;

namespace best_hackathon_2025.Helpers;

public class JwtTokenGenerator
{
    private readonly JwtSettings _cfg;
    private readonly SymmetricSecurityKey _key;

    public JwtTokenGenerator(IOptions<JwtSettings> opt)
    {
        _cfg = opt.Value;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg.Secret));
    }

    public string Generate(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,  user.Id),
            new Claim(JwtRegisteredClaimNames.Email,user.Email),
            new Claim(ClaimTypes.Role,              user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _cfg.Issuer,
            audience: _cfg.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
