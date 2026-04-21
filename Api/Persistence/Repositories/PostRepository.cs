using api.Models;
using api.Persistence.Repositories.Interfaces;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace api.Persistence.Repositories;

public class PostRepository : IPostRepository
{
    private readonly AppDbContext _dbContext;

    public PostRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Post post)
    {
        _dbContext.Posts.Add(post);
    }

    public async Task<ErrorOr<Post>> GetCompleteByIdAsync(int id)
    {
        Post? post = _dbContext.Posts
                                .Include(p => p.PostItems)
                                .ThenInclude(pi => pi.Media)
                                .FirstOrDefault(p => p.Id == id);

        if (post is null) return Error.NotFound("Unexisting post id");

        return post;
    }

    public void Remove(Post post)
    {
        _dbContext.Posts.Remove(post);
    }
}