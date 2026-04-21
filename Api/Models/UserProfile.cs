namespace api.Models;

public class UserProfile
{
    public string DisplayName { get; private set; }
    public string? Bio { get; private set; }
    public UserProfile(string displayName, string? bio = null)
    {
        DisplayName = displayName;
        Bio = bio;
    }
}