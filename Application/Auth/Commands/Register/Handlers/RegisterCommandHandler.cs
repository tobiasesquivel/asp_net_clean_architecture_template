using Application.Auth.Common;
using Application.Auth.Persistence.Repositories;
using Application.Auth.Token;
using Application.Common.Interfaces;
using Cortex.Mediator.Commands;
using Domain.Models.Aggregates;
using Domain.Models.Entities;
using ErrorOr;

namespace Application.Auth.Commands.Register.Handlers;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityService _identityService;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public RegisterCommandHandler(IIdentityService athUserRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, ITokenService tokenService)
    {
        _identityService = athUserRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        try
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            ErrorOr<AuthenticationUser> authUserResult = await _identityService.AddAsync(
                command.Username,
                command.Email,
                command.Password
            );

            if (authUserResult.IsError) return authUserResult.Errors;

            AuthenticationUser authUser = authUserResult.Value;

            UserProfile userProfile = UserProfile.Create(command.DisplayName);
            User user = User.Create(authUser.Id, command.Username, userProfile);
            await _userRepository.AddAsync(user);
            System.Console.WriteLine("eentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acaentro acantro aca");
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            string token = _tokenService.CreateToken(authUser.Id, authUser.Email);

            return new AuthenticationResult(
                new AuthenticationResultUser(
                    authUser.Id,
                    user.Username,
                    authUser.Email,
                    command.DisplayName
                ), token
            );
        }
        catch (Exception)
        {
            return Error.Unexpected("Error inesperado al registrar el usuario");
        }

    }
}