using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BoatBackend.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace BoatBackend.Services;

public class AuthenticationService(UserRepository userRepository)
{
    public async Task<string> Login(string name, string password)
    {
        var user = await userRepository.GetUserByName(name);

        if (user == null || user.Password == password) return string.Empty;

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var jwtKey =
            "TEST"; // NOTE this should be secure with a secret manager or at least in a .env file in a real application.

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var token = new JwtSecurityToken(
            null,
            null,
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}