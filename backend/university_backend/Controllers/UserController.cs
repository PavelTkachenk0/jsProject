using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using university_backend.DAL;
using university_backend.DAL.Models;
using university_backend.Models;
using university_backend.Models.Requests;

namespace university_backend.Controllers;

public class UserController(AppDbContext appDbContext) : Controller
{
    private readonly AppDbContext _appDbContext = appDbContext;

    [HttpPost]
    // [Authorize(Roles = "Admin")]
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
    // [Authorize(Roles = "Admin")]
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

    [HttpPut]
    // [Authorize(Roles = "Admin")]
    [Route("api/admin/users/{id:int}")]
    public async ValueTask<IActionResult> UpdateUser([FromRoute] int id, UpdateUserDTO req, CancellationToken ct)
    {
        if (req == null)
        {
            return BadRequest("Invalid JSON data");
        }

        var query = await _appDbContext.AppUsers.SingleOrDefaultAsync(x => x.Id == id, ct);

        if (query == null)
        {
            return BadRequest("Invalid id");
        }

        if (req.Login != null)
        {
            query.Login = req.Login;
        }
        if (req.Password != null)
        {
            query.Password = req.Password;
        }
        if (req.Role != null)
        {
            var userRoles = req.Role.Split(',');

            var rolesToDelete = _appDbContext.AppUserRoles.Where(x => x.UserId ==  id);
            _appDbContext.RemoveRange(rolesToDelete);

            foreach(var role in userRoles)
            {
                _appDbContext.AppUserRoles.Add(new AppUserRole
                {
                    UserId = id,
                    RoleId = await _appDbContext.AppRoles
                                .Where(x => x.Name == role)
                                .Select(x => x.Id)
                                .SingleAsync(ct)
                });
            }
        }

        _appDbContext.AppUsers.Update(query);

        await _appDbContext.SaveChangesAsync(ct);

        return Ok();
    }
}
