using Domain.Models.Common;

namespace Domain.Models.ValueObjects;

public class CommentId : ValueObject
{
    public Guid Value { get; }

    public static CommentId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    private CommentId(Guid id)
    {
        this.Value = id;
    }

    private CommentId() { }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}