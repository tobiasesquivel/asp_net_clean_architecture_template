using Domain.Models.Common;

namespace Domain.Models.ValueObjects;

public class PublicationLikeId : ValueObject
{
    public Guid Value { get; }

    public static PublicationLikeId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    private PublicationLikeId(Guid id)
    {
        this.Value = id;
    }

    private PublicationLikeId() { }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}