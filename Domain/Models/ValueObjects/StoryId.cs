
using Domain.Models.Common;

namespace Domain.Models.ValueObjects;

public class StoryId : ValueObject
{
    public Guid Value { get; }

    public StoryId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    private StoryId(Guid id)
    {
        this.Value = id;
    }
    private StoryId() { }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}