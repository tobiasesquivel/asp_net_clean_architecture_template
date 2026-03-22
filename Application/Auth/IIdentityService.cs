using Application.Auth.Common;
using ErrorOr;

namespace Application.Auth;

public interface IIdentityService
{
    Task<ErrorOr<AuthenticationUser>> CreateAsync(string username, string email, string password);
    Task<bool> ExistUsernameAsync(string username);
    Task<ErrorOr<AuthenticationUser>> GetByCredentialsAsync(string username, string password);
    Task<ErrorOr<AuthenticationUser>> GetById(string id);
}