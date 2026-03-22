namespace Application.Auth.Common;

public record AuthenticationResult(AuthenticationResultUser User, string Token);