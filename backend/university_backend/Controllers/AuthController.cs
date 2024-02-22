using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace university_backend.Controllers;

[ApiController]
public class AuthenticationController : Controller
{
    private readonly string _secretKey = "1111"; // Замените на ваш секретный ключ
    private readonly string _issuer = "pavel"; // Замените на ваше имя (например, доменное имя)
    private readonly int _expiryMinutes = 60; // Время действия токена (в минутах)

    [HttpPost]
    [Route("api/[controller]")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        if (model.Username == "your_username" && model.Password == "your_password")
        {
            var token = GenerateJwtToken(model.Username);
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }

    private string GenerateJwtToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Name, username)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_expiryMinutes),
            Issuer = _issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class LoginModel
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}
