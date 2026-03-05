using MediatR;

namespace Application.Auth.Login;

public class LoginRequestHandler : IRequestHandler<LoginRequest, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        return new LoginResponse(request.Username, request.Password);
    }
}