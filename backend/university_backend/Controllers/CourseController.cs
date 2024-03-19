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
    [Route("api/course")]
    public async ValueTask<IActionResult> PostCourse([FromBody] CourseDTO req, CancellationToken ct)
    {
        if (req == null)
        {
            return BadRequest("Invalid JSON data");
        }

        var course = new Course
        {
            Name = req.Name,
            Description = req.Description,
            Duration = req.Duration,
            Teacher = req.Teacher
        };

        await _appDbContext.AddAsync(course, ct);

        await _appDbContext.SaveChangesAsync(ct);

        return Ok();
    }

    [HttpGet]
    [Route("api/course/{id:int}")]
    public async ValueTask<CourseResponse?> GetCourse([FromRoute] int id, CancellationToken ct)
    {
        return await _appDbContext.Courses.Where(x => x.Id == id).Select(x => new CourseResponse
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Duration = x.Duration,
            Teacher = x.Teacher
        }).SingleOrDefaultAsync(ct);
    }

    [HttpGet]
    [Route("api/courses")]
    public async ValueTask<CourseResponse[]> GetCourses(CancellationToken ct)
    {
        return await _appDbContext.Courses.Select(x => new CourseResponse
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description,
            Duration = x.Duration,
            Teacher = x.Teacher
        }).ToArrayAsync(ct);
    }

    [HttpDelete]
    [Route("api/course/{id:int}")]
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
