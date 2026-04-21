using api.Enums;
using static api.Mediator.Results.GetPostQueryResult;

namespace api.Mediator.Results;

public record GetPostQueryResult(
    int Id,
    PostAuthor Author,
    DateTime CreatedAt,
    string? Description,
    int LikesCount,
    // int CommentsCount,
    Item[] Items
)
{
    public record PostAuthor(
        string UserId,
        string Username
    );

    public record Photo : Item
    {
        public Photo(int Id, string Url, EMediaTypes Type) : base(Id, Url, Type)
        {
        }
    }

    public record Video : Item
    {
        public int DurationSeconds { get; init; }
        public Video(int Id, string Url, EMediaTypes Type, int DurationSeconds) : base(Id, Url, Type)
        {
            this.DurationSeconds = DurationSeconds;
        }
    }

    public record Item(
        int Id,
        string Url,
        EMediaTypes Type
    );
};
