using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using university_backend.DAL;
using university_backend.DAL.Models;
using university_backend.Models;
using university_backend.Models.Responses;

namespace university_backend.Controllers;


public class ContactController(AppDbContext dbContext) : Controller
{
    private readonly AppDbContext _dbContext = dbContext;

    [HttpPost]
    [Route("api/guest/contact")]
    [Authorize(Roles = "Admin, Guest")]
    public async ValueTask<IActionResult> PostMessage([FromBody] ContactMessageDTO req, CancellationToken ct)
    {
        if (req == null)
        {
            return BadRequest("Invalid JSON data");
        }

        await _dbContext.Messages.AddAsync(new ContactMessage
        {
            Login = HttpContext.User.Identity!.Name!,
            Email = req.Email,
            Message = req.Message
        }, ct);

        await _dbContext.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpGet]
    [Route("api/guest/contacts")]
    [ProducesResponseType<ContactMessageResponse[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Guest")]
    public async ValueTask<IActionResult> GetUserMessages(CancellationToken ct)
    {
        var result = await _dbContext.Messages.Where(x => x.Login == HttpContext.User.Identity!.Name).Select(x => new ContactMessageResponse
        {
            Id = x.Id,
            Login = x.Login,
            Email = x.Email,
            Message = x.Message
        }).ToArrayAsync(ct);

        if (!result.Any())
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    [Route("api/admin/contacts")]
    [ProducesResponseType<ContactMessageResponse[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin")]
    public async ValueTask<IActionResult> GetMessages(CancellationToken ct)
    {
        var result = await _dbContext.Messages.Select(x => new ContactMessageResponse
        {
            Id = x.Id,
            Login = x.Login,
            Email = x.Email,
            Message = x.Message
        }).ToArrayAsync(ct);

        if (!result.Any())
        {
            return NotFound();
        }

        return Ok(result);
    }
}
