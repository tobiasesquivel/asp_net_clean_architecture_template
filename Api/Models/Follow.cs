using api.Interfaces;

namespace api.Models;

public class Follow : Entity
{
    public string FollowerId { get; private set; }
    public string FollowedId { get; private set; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public Follow(string followerId, string followedId)
    {
        FollowerId = followerId;
        FollowedId = followedId;
    }
}