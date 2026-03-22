using Application.Auth.Common;
using Cortex.Mediator.Queries;
using ErrorOr;

namespace Application.Auth.Queries;

public record LoginQuery(string Username, string Password) : IQuery<ErrorOr<AuthenticationResult>>;