using Application.Auth.Common;

namespace Application.Auth.Token;

public interface ITokenService
{
    string CreateToken(string id, string email);
}