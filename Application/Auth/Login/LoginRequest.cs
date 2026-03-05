using MediatR;

namespace Application.Auth.Login;

public record LoginRequest(
    string Username,
    string Password
) : IRequest<LoginResponse>;