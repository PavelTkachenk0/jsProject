﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using university_backend.DAL;
using university_backend.DAL.Models;
using university_backend.Models;

namespace university_backend.Controllers;

public class ContactController(AppDbContext dbContext) : Controller
{
    private readonly AppDbContext _dbContext = dbContext;

    [HttpPost]
    [Route("api/[controller]")]
    public async Task<IActionResult> PostMessage([FromBody] ContactMessageDTO req)
    {
        if (req == null)
        {
            return BadRequest("Invalid JSON data");
        }

        var message = new ContactMessage
        {
            Name = req.Name,
            Email = req.Email,
            Message = req.Message
        };

        _dbContext.Messages.Add(message);

        await _dbContext.SaveChangesAsync();

        return Ok("success");
       
    }

    [HttpGet]
    [Route("api/[controller]")]
    public async Task<ContactMessage[]?> GetMessages()
    {
        var messages = await _dbContext.Messages.ToArrayAsync();

        return messages;
    }

}
