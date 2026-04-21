using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        this.ConfigurePosts(builder);
        this.ConfigurePostItems(builder);
    }

    private void ConfigurePosts(EntityTypeBuilder<Post> builder)
    {
        builder.Property(p => p.Description).HasMaxLength(350);
    }
    private void ConfigurePostItems(EntityTypeBuilder<Post> builder)
    {
        builder.OwnsMany(p => p.PostItems, b =>
        {
            b.ToTable("PostItems");
            b.HasKey(b => b.Id);
            b.Property(b => b.Id).IsRequired().ValueGeneratedOnAdd();

            b.Property("MediaId").IsRequired();
            b.HasOne(b => b.Media).WithOne().HasForeignKey<PostItem>("MediaId");
        });
    }

}