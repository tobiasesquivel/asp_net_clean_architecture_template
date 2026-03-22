using Domain.Models.Common;

namespace Domain.Models.ValueObjects;

public class PublicationId : ValueObject
{
    public Guid Value { get; }

    public static PublicationId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    private PublicationId(Guid id)
    {
        this.Value = id;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}