using Domain.Models.Common;
using Domain.Models.Entities;
using Domain.Models.ValueObjects;

namespace Domain.Models.Aggregates;

public sealed class Comment : AggregateRoot<CommentId>
{
    public UserId UserId { get; private set; }
    public PublicationId PublicationId { get; private set; }
    public CommentId? ParentComment { get; private set; }
    private List<CommentLike> _commentLikes = [];
    public IReadOnlyCollection<CommentLike> CommentLikes => _commentLikes.AsReadOnly();

    public static Comment Create(UserId userId, PublicationId publicationId)
    {
        return new(CommentId.CreateUnique(), userId, publicationId);
    }

    private Comment(CommentId id, UserId userId, PublicationId publicationId) : base(id)
    {
        UserId = userId;
        PublicationId = publicationId;
    }

#pragma warning disable CS8618 
    private Comment() : base() { }
#pragma warning restore CS8618



}