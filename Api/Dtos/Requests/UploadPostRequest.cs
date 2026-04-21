using api.Enums;

namespace api.Dtos.Requests;

public record UploadPostRequest(string UserId, PostItemRequestCreationData[] Items, string? Description = null);

public record PostItemRequestCreationData(string Url, EMediaTypes Type, int Order);