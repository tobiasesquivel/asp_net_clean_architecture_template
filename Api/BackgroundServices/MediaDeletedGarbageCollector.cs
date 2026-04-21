using System.Threading.Channels;
using api.BackgroundServices.Jobs;
using api.Models;
using api.Persistence;

namespace api.BackgroundServices;

public class MediaDeletedGarbageCollector : BackgroundService
{
    private readonly Channel<MediaDeletedGarbageCollectorJob> _channel;
    private readonly IServiceProvider _serviceProvider;

    public MediaDeletedGarbageCollector(Channel<MediaDeletedGarbageCollectorJob> channel, IServiceProvider serviceProvider)
    {
        _channel = channel;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var job in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            using var scope = _serviceProvider.CreateScope();
            AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            Media? media = await dbContext.Set<Media>().FindAsync([job.MediaId], stoppingToken);

            if (media is null) return;

            dbContext.Set<Media>().Remove(media);
            await dbContext.SaveChangesAsync(stoppingToken);
        }
    }
}