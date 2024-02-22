using Microsoft.EntityFrameworkCore;
using university_backend.DAL.Models;

namespace university_backend.DAL;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ContactMessage> Messages { get; set; }
}
