using System.Security.Claims;
using System.Text;
using Application.Auth.Token;
using ErrorOr;
using Infrastructure.Auth.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services;

public class TokenService : ITokenService
{
    private readonly JwtOptions _jwtOptions;
    public TokenService(IOptions<JwtOptions> jwtOptions)
    {
        this._jwtOptions = jwtOptions.Value;
    }

    public string CreateToken(string id, string email)
    {
        string secretKey = this._jwtOptions.SectretKey;
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        List<Claim> claims = [
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.NameId, id)
        ];

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            // The expiration time should be less (Refresh token) 
            Expires = DateTime.Now.AddDays(this._jwtOptions.RefreshTokenExpirationDays),
            SigningCredentials = credentials
        };

        JsonWebTokenHandler handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}
