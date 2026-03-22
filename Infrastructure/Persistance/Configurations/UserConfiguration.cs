using Domain.Models.Aggregates;
using Domain.Models.ValueObjects;
using Infrastructure.Persistance.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUserTable(builder);
        ConfigureUserCommentIdsTable(builder);
        ConfigureUserPostIdsTable(builder);
        ConfigureUserStoryIdTable(builder);
        ConfigureUserFollowerIdsTable(builder);
        ConfigureUserFollowingIdsTable(builder);
        ConfigureUserPublicationLikeIdsTable(builder);
        ConfigureUserCommentLikeIdTable(builder);
    }

    private void ConfigureUserCommentLikeIdTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(u => u.CommentLikes, cl =>
        {
            cl.ToTable("UserCommentLikeIds");

            cl.Property<int>("Id").ValueGeneratedOnAdd().IsRequired();
            cl.HasKey("Id");

            cl.WithOwner().HasForeignKey("UserId");

            cl.Property(l => l.Value).HasColumnName("CommentLikeId").IsRequired().ValueGeneratedNever();
        });

        builder.Metadata.FindNavigation(nameof(User.CommentLikes))!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureUserPublicationLikeIdsTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(u => u.PublicationLikes, pl =>
        {
            pl.ToTable("UserPublicationLikeIds");

            pl.Property<int>("Id").ValueGeneratedOnAdd().IsRequired();
            pl.HasKey("Id");

            pl.WithOwner().HasForeignKey("UserId");

            pl.Property(l => l.Value).HasColumnName("PublicationLikeId").IsRequired().ValueGeneratedNever();
        });

        builder.Metadata.FindNavigation(nameof(User.PublicationLikes))!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureUserFollowingIdsTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(u => u.Following, fb =>
        {
            fb.ToTable("UserFollowingIds");

            fb.Property<int>("Id").ValueGeneratedOnAdd().IsRequired();
            fb.HasKey("Id");

            fb.WithOwner().HasForeignKey("UserId");

            fb.Property(f => f.Value).HasColumnName("FollowingId").IsRequired().ValueGeneratedNever();
        });

        builder.Metadata.FindNavigation(nameof(User.Following))!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureUserFollowerIdsTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(u => u.Followers, fb =>
        {
            fb.ToTable("UserFollowersIds");

            fb.Property<int>("Id").ValueGeneratedOnAdd().IsRequired();
            fb.HasKey("Id");

            fb.WithOwner().HasForeignKey("UserId");

            fb.Property(f => f.Value).HasColumnName("FollowerId").IsRequired().ValueGeneratedNever();
        });

        builder.Metadata.FindNavigation(nameof(User.Followers))!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureUserStoryIdTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(u => u.Stories, sb =>
        {
            sb.ToTable("UserStoryIds");

            sb.Property<int>("Id").ValueGeneratedOnAdd().IsRequired();
            sb.HasKey("Id");

            sb.WithOwner().HasForeignKey("UserId");

            sb.Property(s => s.Value).HasColumnName("StoryId").IsRequired();
        });

        builder.Metadata.FindNavigation(nameof(User.Stories))!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private void ConfigureUserPostIdsTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(u => u.Posts, pb =>
        {
            pb.ToTable("UserPostIds");

            pb.Property<int>("Id").IsRequired().ValueGeneratedOnAdd();
            pb.HasKey("Id");

            pb.WithOwner().HasForeignKey("UserId");

            pb.Property(p => p.Value).HasColumnName("PostId").ValueGeneratedNever().IsRequired();
        });

        builder.Metadata.FindNavigation(nameof(User.Posts))!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
    public void ConfigureUserCommentIdsTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(u => u.Comments, cb =>
        {
            cb.ToTable("UserCommentIds");

            cb.Property<int>("Id").ValueGeneratedOnAdd();
            cb.HasKey("Id");

            cb.WithOwner().HasForeignKey("UserId");

            cb.Property(c => c.Value).HasColumnName("CommentId")
            .ValueGeneratedNever()
            .IsRequired();
        });

        builder.Metadata.FindNavigation(nameof(User.Comments))!
                        .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    public void ConfigureUserTable(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasOne<AuthUser>()
               .WithOne()
               .HasForeignKey<User>(u => u.AuthUserId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);


        builder.Property(u => u.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => UserId.Create(value)
            );

        builder.Property(u => u.Username).IsRequired().HasMaxLength(30);

        builder.OwnsOne(u => u.Profile, profileBuilder =>
        {
            profileBuilder.Property(p => p.DisplayName).HasMaxLength(25);
            profileBuilder.Property(p => p.Bio).HasMaxLength(300);
        });


    }


}