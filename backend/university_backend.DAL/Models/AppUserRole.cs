using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace university_backend.DAL.Models;

public class AppUserRole
{
    public int UserId { get; set; }
    public int RoleId {  get; set; }

    public AppUser? User { get; set; }
    public AppRole? Role { get; set; }
}

public class AppUserRoleConfig : IEntityTypeConfiguration<AppUserRole>
{
    public void Configure(EntityTypeBuilder<AppUserRole> builder)
    {
        builder.HasKey(x => new { x.UserId, x.RoleId });
    }
}
