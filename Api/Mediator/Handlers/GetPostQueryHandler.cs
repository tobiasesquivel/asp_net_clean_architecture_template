using api.Enums;
using api.Mediator.Queries;
using api.Mediator.Results;
using api.Models;
using api.Persistence;
using ErrorOr;
using ImTools;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace api.Mediator.Handlers;

public class GetPostQueryHandler
{
    private readonly AppDbContext _dbContext;

    public GetPostQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ErrorOr<GetPostQueryResult>> Handle(GetPostQuery query, CancellationToken cancellationToken)
    {
        ErrorOr<GetPostQueryResult> resultWithBaseMediasResult = await this.GetResultWithGenericMedias(query, cancellationToken);

        if (resultWithBaseMediasResult.IsError) return resultWithBaseMediasResult.Errors;

        GetPostQueryResult resultWithBaseMedias = resultWithBaseMediasResult.Value;

        ErrorOr<List<GetPostQueryResult.Item>> replacedItemsResult = await FindReplaceItemsWithConcretesAsync(resultWithBaseMedias.Items, cancellationToken);

        if (replacedItemsResult.IsError) return replacedItemsResult.Errors;

        GetPostQueryResult.Item[] replacedItems = replacedItemsResult.Value.ToArray();

        GetPostQueryResult result = resultWithBaseMedias with { Items = replacedItems };
        return result;
    }

    private async Task<ErrorOr<List<GetPostQueryResult.Item>>> FindReplaceItemsWithConcretesAsync(
        GetPostQueryResult.Item[] items,
        CancellationToken cancellationToken)
    {
        var itemsGroupedByType = items.GroupBy(i => i.Type);
        List<GetPostQueryResult.Item> concreteItems = [];

        foreach (var group in itemsGroupedByType)
        {
            int[] groupIds = group.Select(item => item.Id).ToArray();

            List<GetPostQueryResult.Item>? concreteMedias = group.Key switch
            {
                EMediaTypes.Photo => (await _dbContext.Set<Photo>()
                                               .Where(p => groupIds.Contains(p.Id))
                                               .ToListAsync(cancellationToken))
                                     .Select(p =>
                                         (GetPostQueryResult.Item)new GetPostQueryResult.Photo(p.Id, p.Url, p.Type)
                                     ).ToList(),

                EMediaTypes.Video => (await _dbContext.Set<Video>()
                                               .Where(v => groupIds.Contains(v.Id))
                                               .ToListAsync(cancellationToken))
                                     .Select(v =>
                                         (GetPostQueryResult.Item)new GetPostQueryResult.Video(v.Id, v.Url, v.Type, v.DurationSeconds)
                                     ).ToList(),

                _ => null
            };

            if (concreteMedias is null)
                return Error.Unexpected(description: $"The media type {group.Key} is not implemented for querying.");

            concreteItems.AddRange(concreteMedias);
        }

        return concreteItems;
    }



    private async Task<ErrorOr<GetPostQueryResult>> GetResultWithGenericMedias(GetPostQuery query, CancellationToken cancellationToken)
    {
        GetPostQueryResult? result = await _dbContext.Posts
                       .AsNoTracking()
                       .Where(p => p.Id == query.PostId)
                       .Select(
                           p => new GetPostQueryResult(
                               query.PostId,
                               new GetPostQueryResult.PostAuthor(
                                   p.User.AuthId,
                                   p.User.Username
                               ),
                               DateTime.UtcNow,
                               p.Description,
                               p.LikesCount,
                               p.PostItems.Select(
                                   pi => new GetPostQueryResult.Item(
                                       pi.Media.Id,
                                       pi.Media.Url,
                                       pi.Media.Type
                                   )
                               ).ToArray()
                           )
                       ).FirstOrDefaultAsync(cancellationToken: cancellationToken);


        if (result is null) return Error.NotFound(description: "Post not founded");

        return result;
    }
}