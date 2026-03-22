using Application.Auth.Common;
using Cortex.Mediator.Commands;
using ErrorOr;

namespace Application.Auth.Commands.Register;

public record RegisterCommand(string Username, string Email, string DisplayName, string Password) : ICommand<ErrorOr<AuthenticationResult>>;