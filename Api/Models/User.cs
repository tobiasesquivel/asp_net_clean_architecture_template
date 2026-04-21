using api.Interfaces;

namespace api.Models;

public class User : Entity
{
    public string AuthId { get; private set; }
    public string Username { get; private set; }
    public UserProfile UserProfile { get; private set; }

    public int FollowersCount { get; private set; } = 0;
    public int FollowingCount { get; private set; } = 0;
    public User(string authId, string username, UserProfile userProfile)
    {
        AuthId = authId;
        Username = username;
        UserProfile = userProfile;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private User() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}