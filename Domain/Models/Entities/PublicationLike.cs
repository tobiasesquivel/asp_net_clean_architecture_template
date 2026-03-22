using Domain.Models.Common;
using Domain.Models.ValueObjects;

namespace Domain.Models.Entities;

public class PublicationLike : Entity<PublicationLikeId>
{
    public UserId UserId { get; private set; }
    public PublicationId PublicationId { get; private set; }

    public static PublicationLike Create(UserId userId, PublicationId publicationId)
    {
        return new(PublicationLikeId.CreateUnique(), userId, publicationId);
    }

    private PublicationLike(PublicationLikeId id, UserId userId, PublicationId publicationId) : base(id)
    {
        this.UserId = userId;
        this.PublicationId = publicationId;
    }

#pragma warning disable 8618
    private PublicationLike() : base()
    {

    }
#pragma warning restore 8618

}