using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Persistence.Configurations;

public class PublicationConfiguration : IEntityTypeConfiguration<Publication>
{
    public void Configure(EntityTypeBuilder<Publication> builder)
    {
        builder.UseTptMappingStrategy();
    }

    public void ConfigureCommentsTable(EntityTypeBuilder<Publication> builder)
    {
        builder.OwnsMany(p => p.Comments, b =>
        {
            b.ToTable("Comments");

            b.HasKey(c => c.Id);

            b.HasOne(c => c.ParentComment)
            .WithMany(pc => pc.Replies)
            .HasForeignKey()
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
        });
    }
}