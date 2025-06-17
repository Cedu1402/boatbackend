using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BoatBackend.Interfaces;
using BoatBackend.Models;
using Microsoft.IdentityModel.Tokens;

namespace BoatBackend.Services;

public class AuthenticationService(IUserRepository userRepository, ILogger<AuthenticationService> logger)
{
    public static readonly string JwtKey =
        "TESTKEEEEEEEEEEEEYYYYYYYYYYYYYYYYYYYYYYYYYYY"; // NOTE this should be secure with a secret manager or at least in a .env file in a real application.

    public async Task<string> Login(User user)
    {
        var existingUser = await userRepository.GetUserByName(user.Name);

        if (existingUser == null || existingUser.Password != user.Password)
        {
            logger.LogInformation("Failed to login {UserName}", user.Name);
            return string.Empty;
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
        var token = new JwtSecurityToken(
            null,
            null,
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}