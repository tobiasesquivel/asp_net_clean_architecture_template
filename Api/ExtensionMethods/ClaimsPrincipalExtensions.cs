using System.Security.Claims;
using ErrorOr;

namespace api.ExtensionMethods;

public static class ClaimsPrincipalExtensions
{
    public static ErrorOr<string> GetUserId(this ClaimsPrincipal principal)
    {
        if (principal is null) return Error.Validation(description: "UserId cannot be null");

        Claim? claim = principal.FindFirst(ClaimTypes.NameIdentifier);

        if (claim is null) return Error.Validation(description: "User doesnt have nameIdentifier claim");

        return claim.Value;
    }
}