using Microsoft.EntityFrameworkCore;
using university_backend.DAL.Models;

namespace university_backend.DAL;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ContactMessage> Messages { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<AppUserRole> AppUserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
