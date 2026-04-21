using api.Models;
using api.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.AuthId);
        builder.HasOne<AppIdentityUser>().WithOne().HasForeignKey<User>(u => u.AuthId).OnDelete(DeleteBehavior.Cascade);

        ConfigureUserProfile(builder);
    }

    public void ConfigureUserProfile(EntityTypeBuilder<User> builder)
    {
        builder.OwnsOne(u => u.UserProfile, b =>
        {
            b.Property(p => p.Bio).HasColumnName(nameof(UserProfile.Bio));
            b.Property(p => p.DisplayName).HasColumnName(nameof(UserProfile.DisplayName));
        });
    }
}