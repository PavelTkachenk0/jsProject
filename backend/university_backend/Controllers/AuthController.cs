using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using university_backend.DAL;
using university_backend.DAL.Models;
using university_backend.Models;
using university_backend.Models.Responses;

namespace university_backend.Controllers;

public class AuthenticationController(AppDbContext appDbContext) : Controller
{
    private readonly AppDbContext _appDbContext = appDbContext;

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Route("api/auth/login")]
    public async Task<IActionResult> Login([FromBody] UserDTO model, CancellationToken ct)
    {
        var user = await _appDbContext.AppUsers
            .Where(x => x.Login == model.Login)
            .Select(x => new
            {
                x.Login,
                Roles = x.Roles
                    .Select(x => x.Name)
                    .ToArray()
            }).SingleOrDefaultAsync(ct);

        if(user == null)
        {
            return Unauthorized();
        }

        if(!user.Roles.Any())
        {
            return Unauthorized();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Login)
        };

        var roleClaims = user.Roles.Select(role => new Claim(ClaimTypes.Role, role));

        claims.AddRange(roleClaims);

        ClaimsIdentity claimsIdentity = new(claims, "Cookies");

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        var response = new AuthResponse
        {
            Login = model.Login,
            Password = model.Password,
            Roles = user.Roles.Select(role => new Claim(ClaimTypes.Role, role).Value).ToArray()
        };

        return Ok(response);
    }

    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType<AuthResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Route("api/auth/register")]
    public async Task<IActionResult> Register([FromBody] UserDTO model, CancellationToken ct)
    {
        var regUser = new AppUser
        {
            Login = model.Login,
            Password = model.Password,
        };

        var userEntity = await _appDbContext.AppUsers.AddAsync(regUser, ct);

        await _appDbContext.SaveChangesAsync(ct);

        await _appDbContext.AppUserRoles
            .AddAsync(new AppUserRole
            {
                UserId = userEntity.Entity.Id,
                RoleId = 1
            }, ct);

        await _appDbContext.SaveChangesAsync(ct);

        return await Login(model, ct);
    }

    [HttpPost]
    [Route("api/auth/logout")]
    [Authorize(Roles = "Admin, Guest")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Redirect("/api/auth/login");
    }
}
