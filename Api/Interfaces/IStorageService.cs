using ErrorOr;

namespace api.Interfaces;

public interface IStorageService
{
    Task<ErrorOr<List<string>>> UploadManyAsync(IFormFile[] files, CancellationToken? cancellationToken = null);
}