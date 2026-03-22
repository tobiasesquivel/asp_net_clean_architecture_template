namespace Api.Dtos.Requests;

public record RegisterRequest(string Username, string Email, string DisplayName, string Password);