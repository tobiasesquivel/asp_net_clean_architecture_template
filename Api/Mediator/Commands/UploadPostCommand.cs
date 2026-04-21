namespace api.Mediator.Commands;

public record UploadPostCommand(string UserId, ICollection<string> MediaUrls, string? Description = null);
