using System.Threading.Channels;
using api.BackgroundServices.Jobs;
using api.Models;
using api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace api.Mediator.Events.Handlers;

public class PostDeletedEventHandler
{
    private readonly AppDbContext _dbContext;
    private readonly Channel<MediaDeletedGarbageCollectorJob> _channel;

    public PostDeletedEventHandler(Channel<MediaDeletedGarbageCollectorJob> channel, AppDbContext dbContext)
    {
        _channel = channel;
        _dbContext = dbContext;
    }

    public async Task Handle(PostDeletedEvent notification, CancellationToken cancellationToken)
    {
        await DeletePostMediasGarbageCollector(notification.Post);
    }

    private async Task DeletePostMediasGarbageCollector(Post post)
    {

        List<PostItem> postItems = (await this._dbContext.Posts
                                            .Include(p => p.PostItems)
                                            .ThenInclude(pi => pi.Media)
                                            .FirstAsync(p => p.Id == post.Id))
                                                .PostItems.ToList();

        List<Media> medias = postItems.Select(pi => pi.Media).ToList();

        foreach (Media media in medias)
        {
            MediaDeletedGarbageCollectorJob job = new(media.Id);
            await _channel.Writer.WriteAsync(job);
        }
    }

}