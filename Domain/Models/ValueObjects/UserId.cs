using Domain.Models.Common;

namespace Domain.Models.ValueObjects;

public class UserId : ValueObject
{
    public Guid Value { get; }

    public static UserId Create(Guid id)
    {
        return new(id);
    }
    public static UserId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    private UserId(Guid id)
    {
        this.Value = id;
    }

    private UserId() { }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}