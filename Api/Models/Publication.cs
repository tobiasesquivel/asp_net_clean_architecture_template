using api.Interfaces;

namespace api.Models;

public abstract class Publication : Entity
{
    public int Id { get; private set; }
    public string UserId { get; private set; }
    public User User { get; private set; } = null!;
    private List<Comment> _comments = [];
    public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    protected Publication(string userId)
    {
        UserId = userId;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    protected Publication() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}