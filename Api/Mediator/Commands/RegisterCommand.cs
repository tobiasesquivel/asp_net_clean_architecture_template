namespace api.Mediator.Commands;

public record RegisterCommand(string Username, string Email, string DisplayName, string Password);