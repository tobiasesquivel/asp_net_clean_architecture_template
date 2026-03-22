using Domain.Models.Common;
using Domain.Models.Entities;
using Domain.Models.ValueObjects;

namespace Domain.Models.Aggregates;

public class User : AggregateRoot<UserId>
{
    public string AuthUserId { get; private set; }
    public string Username { get; private set; }
    public UserProfile Profile { get; private set; }
    private List<CommentId> _comments = [];
    private List<PostId> _posts = [];
    private List<StoryId> _stories = [];
    private List<UserId> _followers = [];
    private List<UserId> _following = [];
    private List<PublicationLikeId> _publicationLikes = [];
    private List<CommentLikeId> _commentLikes = [];

    public IReadOnlyCollection<CommentId> Comments => _comments.AsReadOnly();
    public IReadOnlyCollection<PostId> Posts => _posts.AsReadOnly();
    public IReadOnlyCollection<StoryId> Stories => _stories.AsReadOnly();
    public IReadOnlyCollection<UserId> Followers => _followers.AsReadOnly();
    public IReadOnlyCollection<UserId> Following => _following.AsReadOnly();
    public IReadOnlyCollection<PublicationLikeId> PublicationLikes => _publicationLikes.AsReadOnly();
    public IReadOnlyCollection<CommentLikeId> CommentLikes => _commentLikes.AsReadOnly();

    public static User Create(string authUserId, string username, UserProfile profile)
    {
        return new(UserId.CreateUnique(), username, authUserId, profile);
    }
    private User(UserId id, string username, string authUserId, UserProfile profile) : base(id)
    {
        this.Username = username;
        this.Profile = profile;
        this.AuthUserId = authUserId;
    }


#pragma warning disable CS8618
    private User() : base() { }
#pragma warning restore CS8618
}