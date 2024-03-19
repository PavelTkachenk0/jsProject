using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using university_backend.DAL;
using university_backend.DAL.Models;
using university_backend.Models;
using university_backend.Models.Responses;

namespace university_backend.Controllers;

public class CourseController(AppDbContext dbContext) : Controller
{
    private readonly AppDbContext _appDbContext = dbContext;

    [HttpPost]
    [Route("api/admin/course")]
    [Authorize(Roles = "Admin")]
    public async ValueTask<IActionResult> PostCourse([FromBody] CourseDTO req, CancellationToken ct)
    {
        if (req == null)
        {
            return BadRequest("Invalid JSON data");
        }

        await _appDbContext.AddAsync(new Course
        {
            Name = req.Name,
            Description = req.Description,
            Duration = req.Duration,
            Teacher = req.Teacher
        }, ct);

        await _appDbContext.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpGet]
    [Route("api/admin/course/{id:int}")]
    [ProducesResponseType<CourseResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin")]
    public async ValueTask<IActionResult> GetCourse([FromRoute] int id, CancellationToken ct)
    {
        var result =  await _appDbContext.Courses.Where(x => x.Id == id).Select(x => new CourseResponse
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Duration = x.Duration,
            Teacher = x.Teacher
        }).SingleOrDefaultAsync(ct);

        if(result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet]
    [Route("api/courses")]
    [ProducesResponseType<CourseResponse[]>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin, Guest")]
    public async ValueTask<IActionResult> GetCourses(CancellationToken ct)
    {
        var result = await _appDbContext.Courses.Select(x => new CourseResponse
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Duration = x.Duration,
            Teacher = x.Teacher
        }).ToArrayAsync(ct);
    
        if (!result.Any())
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete]
    [Route("api/admin/course/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async ValueTask<IActionResult> DeleteCourse([FromRoute] int id, CancellationToken ct)
    {
        var result = await _appDbContext.Courses.SingleOrDefaultAsync(x => x.Id == id, ct);

        if(result == null)
        {
            return NotFound();
        }

        _appDbContext.Remove(result);

        await _appDbContext.SaveChangesAsync(ct);

        return Ok();
    }
}
