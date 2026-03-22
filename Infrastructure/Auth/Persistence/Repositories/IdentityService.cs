using Application.Auth;
using Application.Auth.Common;
using ErrorOr;
using Infrastructure.Persistance.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Auth.Persistence.Repositories;

public class IdentityService : IIdentityService
{
    private readonly UserManager<AuthUser> _userManager;

    public IdentityService(UserManager<AuthUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ErrorOr<AuthenticationUser>> CreateAsync(string username, string email, string password)
    {
        AuthUser user = new() { UserName = username, Email = email };
        ErrorOr<IdentityResult> registerResult = await _userManager.CreateAsync(user, password);

        if (registerResult.Errors.Count != 0)
        {
            foreach (Error error in registerResult.Errors)
            {
                System.Console.WriteLine(error.Description);
            }
            return Error.Conflict(description: "Error en identityservice");
        }

        return new AuthenticationUser(
            user.Id,
            username,
            email
        );
    }

    public Task<bool> ExistUsernameAsync(string username)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<AuthenticationUser>> GetByCredentialsAsync(string username, string password)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<AuthenticationUser>> GetById(string id)
    {
        throw new NotImplementedException();
    }

}