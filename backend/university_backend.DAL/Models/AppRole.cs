using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using university_backend.DAL.Constants;

namespace university_backend.DAL.Models;

public class AppRole
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<AppUser> Users { get; set; } = new HashSet<AppUser>();
    public ICollection<AppUserRole> UserRoles { get; set; } = new HashSet<AppUserRole>();
}

public class AppRoleConfig : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.Property(x => x.Name).HasMaxLength(128);

        builder.HasData(
            new AppRole { Id = 1, Name = Roles.Guest},
            new AppRole { Id = 2, Name = Roles.Admin}
        );
    }
}
