using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace university_backend.DAL.Models;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; } 
    public int Duration { get; set; } 
    public string Teacher { get; set; } = null!; 
}

public class CourseConfig : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(256);
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.Teacher).HasMaxLength(256);
    }
}