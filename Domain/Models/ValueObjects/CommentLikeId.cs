using Domain.Models.Common;

namespace Domain.Models.ValueObjects;

public class CommentLikeId : ValueObject
{
    public Guid Value { get; }

    public static CommentLikeId CreateUnique()
    {
        return new(Guid.NewGuid());
    }

    private CommentLikeId(Guid id)
    {
        this.Value = id;
    }

    private CommentLikeId() { }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}