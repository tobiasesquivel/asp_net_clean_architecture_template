using System.Runtime.InteropServices;
using Domain.Models.Common;
using Domain.Models.ValueObjects;

namespace Domain.Models.Entities;

public class UserProfile : ValueObject
{
    public string DisplayName { get; private set; }
    public string? Bio { get; private set; }

    public static UserProfile Create(string displayName, string? bio = null)
    {
        return new(displayName, bio);
    }

    private UserProfile(string displayName, string? bio)
    {
        this.DisplayName = displayName;
        this.Bio = bio;
    }

#pragma warning disable CS8618
    private UserProfile() { }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
#pragma warning restore CS8618
}