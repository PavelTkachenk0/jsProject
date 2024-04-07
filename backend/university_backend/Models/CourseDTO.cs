namespace university_backend.Models;

public class CourseDTO
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; } 
    public int Duration { get; set; } 
    public string Teacher { get; set; } = null!;
}

public class UpdateCourseDTO
{
    public string? Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? Duration { get; set; }
    public string? Teacher { get; set; } = null!;
}