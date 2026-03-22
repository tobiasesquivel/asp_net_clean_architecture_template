using Domain.Models.Common;
using Domain.Models.ValueObjects;

namespace Domain.Models.Entities;

public class CommentLike : Entity<CommentLikeId>
{
    public UserId UserId { get; private set; }
    public CommentId CommentId { get; private set; }

    public static CommentLike Create(UserId userId, CommentId commentId)
    {
        return new(CommentLikeId.CreateUnique(), userId, commentId);
    }

    private CommentLike(CommentLikeId id, UserId userId, CommentId commentId) : base(id)
    {
        this.UserId = userId;
        this.CommentId = commentId;
    }

#pragma warning disable 8618
    private CommentLike() : base()
    {

    }
#pragma warning restore 8618

}