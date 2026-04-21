using api.Mediator.Commands;
using api.Persistence.Repositories.Interfaces;
using FluentValidation;

namespace api.Mediator.Validation.Validators;

public class UploadPostCommandValidator : AbstractValidator<UploadPostCommand>
{
    public UploadPostCommandValidator()
    {
        RuleFor(c => c.UserId)
        .NotEmpty()
        .WithMessage("UserId cannot be empty");
    }

}