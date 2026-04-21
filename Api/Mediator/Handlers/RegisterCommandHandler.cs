using api.Mediator.Commands;
using api.Models;
using api.Models.Common;
using api.Persistence.Repositories.Interfaces;
using api.Services;
using ErrorOr;

namespace api.Mediator.Handlers;

public class RegisterCommandHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IdentityService _IdentityService;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUnitOfWork unitOfWork, IdentityService identityService, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _IdentityService = identityService;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<AuthenticatedUser>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        ErrorOr<CreatedUser> createdUserResult = await _IdentityService.CreateUser(command.Username, command.Email, command.Password);

        if (createdUserResult.IsError) return Error.Failure(description: "Error at creating identity user");

        CreatedUser createdUser = createdUserResult.Value;

        User user = new(
            createdUser.Id,
            createdUser.Username,
            new UserProfile(command.DisplayName)
        );
        _userRepository.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);
        return new AuthenticatedUser(user.Username,
                                     createdUser.Email,
                                     user.UserProfile.DisplayName);
    }
}