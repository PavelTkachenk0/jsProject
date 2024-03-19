using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace university_backend.DAL.Models;

public class AppUser
{
    public int Id { get; set; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;

    public ICollection<AppUserRole> UserRoles { get; set; } = new HashSet<AppUserRole>();
    public ICollection<AppRole> Roles { get; set; } = new HashSet<AppRole>();
}

public class AppUserConfig : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(x => x.Login).HasMaxLength(256);
        builder.Property(x => x.Password).HasMaxLength(128);

        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<AppUserRole>(
                userRole => userRole
                    .HasOne(x => x.Role)
                    .WithMany(x => x.UserRoles)
                    .HasForeignKey(x => x.RoleId),
                userRole => userRole
                    .HasOne(x => x.User)
                    .WithMany(x => x.UserRoles)
                    .HasForeignKey(x => x.UserId)
            );
    }
}
