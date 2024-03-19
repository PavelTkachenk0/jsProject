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
    [Route("api/contact")]
    public async ValueTask<IActionResult> PostMessage([FromBody] ContactMessageDTO req, CancellationToken ct)
    {
        if (req == null)
        {
            return BadRequest("Invalid JSON data");
        }

        var message = new ContactMessage
        {
            Name = "",
            Email = req.Email,
            Message = req.Message
        };

        await _dbContext.Messages.AddAsync(message, ct);

        await _dbContext.SaveChangesAsync(ct);

        return Ok();
       
    }

    [HttpGet]
    [Route("api/contacts")]
    public async ValueTask<ContactMessageResponse[]?> GetMessages(CancellationToken ct)
    {
        return await _dbContext.Messages.Select(x => new ContactMessageResponse
        {
            Id = x.Id,
            Name = x.Name,
            Email = x.Email,
            Message = x.Message
        }).ToArrayAsync(ct);

    }
}
