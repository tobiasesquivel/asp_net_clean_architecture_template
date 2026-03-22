using Domain.Models.Common;

namespace Domain.Models.ValueObjects;

public class UserProfileId : ValueObject
{
    public Guid Value { get; }

    public static UserProfileId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    private UserProfileId(Guid id)
    {
        this.Value = id;
    }

    private UserProfileId() { }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}