
using Domain.Models.Common;

namespace Domain.Models.ValueObjects;

public class PostId : ValueObject
{
    public Guid Value { get; }

    public PostId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    private PostId(Guid id)
    {
        this.Value = id;
    }

    private PostId() { }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}