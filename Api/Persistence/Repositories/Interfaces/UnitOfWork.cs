using api.Interfaces;

namespace api.Persistence.Repositories.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = _dbContext.ChangeTracker.Entries<Entity>();
        Console.WriteLine(entries);
        int newEntriesCount = await _dbContext.SaveChangesAsync(cancellationToken);
        return newEntriesCount;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_dbContext.Database.CurrentTransaction == null)
        {
            await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_dbContext.Database.CurrentTransaction != null)
        {
            await _dbContext.Database.CommitTransactionAsync(cancellationToken);
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_dbContext.Database.CurrentTransaction != null)
        {
            await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
        }
    }

}