using api.Mediator.Commands;
using api.Models;
using api.Persistence;
using api.Persistence.Repositories.Interfaces;
using ErrorOr;

namespace api.Mediator.Handlers;

public class DeletePostCommandHandler
{
    private readonly AppDbContext _dbContext;
    private readonly IPostRepository _postRepository;

    public DeletePostCommandHandler(AppDbContext dbContext, IPostRepository postRepository)
    {
        _dbContext = dbContext;
        _postRepository = postRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeletePostCommand command, CancellationToken cancellationToken)
    {
        ErrorOr<Post> postResult = await _postRepository.GetCompleteByIdAsync(command.PostId);
        if (postResult.IsError) return postResult.Errors;
        Post post = postResult.Value;
        post.Delete();
        _postRepository.Remove(post);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}