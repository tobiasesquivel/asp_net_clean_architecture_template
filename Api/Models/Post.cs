using api.Mediator.Events;
using ErrorOr;

namespace api.Models;

public class Post : Publication
{
    public string? Description { get; private set; }
    public int LikesCount { get; private set; } = 0;

    public IReadOnlyCollection<PostItem> PostItems => _postItems.AsReadOnly();
    private readonly List<PostItem> _postItems = [];

    public static ErrorOr<Post> Create(string userId, IEnumerable<PostItem> postItems, string? description = null)
    {
        return new Post(userId, postItems, description);
    }

    private Post(string userId, IEnumerable<PostItem> postItems, string? description = null) : base(userId)
    {
        this.Description = description;
        this._postItems = postItems.ToList();
        AddDomainEvent(new PostCreatedEvent(new Post(userId, postItems, description)));
    }

    public void Delete()
    {
        this.AddDomainEvent(new PostDeletedEvent(this));
    }

    private Post() { }
}