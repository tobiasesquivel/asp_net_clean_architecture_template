using Domain.Models.Common;
using Domain.Models.Entities;
using Domain.Models.ValueObjects;

namespace Domain.Models.Aggregates;

public abstract class Publication : AggregateRoot<PublicationId>
{
    public UserId UserId { get; protected set; }
    public DateTime CreatedAt { get; protected init; } = DateTime.UtcNow;
    private List<CommentId> _commentIds { get; set; } = [];
    public IReadOnlyCollection<CommentId> Comments => _commentIds.AsReadOnly();
    private List<PublicationLike> _publicationLikes { get; set; } = [];
    public IReadOnlyCollection<PublicationLike> Likes => _publicationLikes.AsReadOnly();

    protected Publication(PublicationId id, UserId userId) : base(id)
    {
        this.UserId = userId;
    }

#pragma warning disable CS8618 
    protected Publication() : base()
    {

    }
#pragma warning restore CS8618
}