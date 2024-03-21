using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using university_backend.DAL;
using university_backend.DAL.Models;
using university_backend.Models.Requests;

namespace university_backend.Controllers;

public class UserController(AppDbContext appDbContext) : Controller
{
    private readonly AppDbContext _appDbContext = appDbContext;

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Route("api/admin/users/register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRequest model, CancellationToken ct)
    {
        var roles = new List<string>
        {
            "Admin",
            "Guest"
        };

        if (!roles.Contains(model.Role))
        {
            return BadRequest("Role is not valid");
        }

        if (_appDbContext.AppUsers.Where(x => x.Login == model.Login).Any())
        {
            await _appDbContext.AppUserRoles
            .AddAsync(new AppUserRole
                {
                    UserId = await _appDbContext.AppUsers
                        .Where(x => x.Login == model.Login)
                        .Select(x => x.Id)
                        .SingleAsync(ct),
                    RoleId = await _appDbContext.AppRoles
                        .Where(x => x.Name == model.Role)
                        .Select(x => x.Id)
                        .SingleAsync(ct)
                }, ct);

            await _appDbContext.SaveChangesAsync(ct);
            return Ok();
        }
        else
        {
            if (model.Password == null)
            {
                return BadRequest("password is not valid");
            }

            var regUser = new AppUser
            {
                Login = model.Login,
                Password = model.Password!,
            };

            var userEntity = await _appDbContext.AppUsers.AddAsync(regUser, ct);

            await _appDbContext.SaveChangesAsync(ct);

            await _appDbContext.AppUserRoles
                .AddAsync(new AppUserRole
                {
                    UserId = userEntity.Entity.Id,
                    RoleId = await _appDbContext.AppRoles
                        .Where(x => x.Name == model.Role)
                        .Select(x => x.Id)
                        .SingleAsync(ct)
                }, ct);

            await _appDbContext.SaveChangesAsync(ct);

            return Ok();
        }
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Route("api/admin/users/{id:int}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id, CancellationToken ct)
    {
        var result = await _appDbContext.AppUsers.FindAsync(id, ct);

        if (result == null)
        {
            return NotFound();
        }

        _appDbContext.AppUsers.Remove(result);

        await _appDbContext.SaveChangesAsync(ct);

        return Ok();
    }
}
