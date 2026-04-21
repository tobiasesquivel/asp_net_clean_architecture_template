namespace api.Dtos.Requests;

public record RegisterRequest(string Username, string Email, string Displayname, string Password);