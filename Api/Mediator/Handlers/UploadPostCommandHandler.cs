using api.Mediator.Commands;
using api.Models;
using api.Persistence.Repositories.Interfaces;
using ErrorOr;

namespace api.Mediator.Handlers;

public class UploadPostCommandHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public UploadPostCommandHandler(IUnitOfWork unitOfWork, IPostRepository postRepository, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<int>> Handle(UploadPostCommand command, CancellationToken cancellationToken)
    {
        if (!await _userRepository.ExistsAsync(command.UserId)) return Error.NotFound("Unexisting UserId");

        Photo[] photos = command.MediaUrls.Select(m => new Photo(m)).ToArray();
        PostItem[] postItems = photos.Select(p => new PostItem(p, photos.IndexOf(p))).ToArray();

        ErrorOr<Post> postResult = Post.Create(
            command.UserId,
            postItems,
            command.Description
        );

        if (postResult.IsError) return postResult.Errors;

        Post post = postResult.Value;

        _postRepository.Add(post);

        await _unitOfWork.SaveChangesAsync();

        return post.Id;
    }

}