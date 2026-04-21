using api.Models;
using ErrorOr;

namespace api.Persistence.Repositories.Interfaces;

public interface IPostRepository
{
    public void Add(Post post);

    public Task<ErrorOr<Post>> GetCompleteByIdAsync(int id);
    public void Remove(Post post);
}