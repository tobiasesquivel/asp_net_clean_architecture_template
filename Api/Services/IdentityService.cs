using api.Models.Common;
using api.Persistence.Identity;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace api.Services;

public class IdentityService
{
    private readonly UserManager<AppIdentityUser> _userManager;

    public IdentityService(UserManager<AppIdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ErrorOr<CreatedUser>> CreateUser(string username, string email, string password)
    {
        AppIdentityUser identityUser = new() { UserName = username, Email = email };
        IdentityResult result = await _userManager.CreateAsync(identityUser, password);

        if (!result.Succeeded) { return Error.Unexpected(description: "Error al crear el usuario"); }

        return new CreatedUser(
            identityUser.Id,
            identityUser.UserName,
            identityUser.Email
        );
    }
}