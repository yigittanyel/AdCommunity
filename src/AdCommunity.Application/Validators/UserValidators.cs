using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Features.User.Commands.CreateUserCommand;
using AdCommunity.Application.Features.User.Commands.UpdateUserCommand;
using FluentValidation;

namespace AdCommunity.Application.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.LastName).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Email).NotEmpty().EmailAddress();
        RuleFor(dto => dto.Phone).NotEmpty().MaximumLength(15);
        RuleFor(dto => dto.Username).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Facebook).MaximumLength(50);
        RuleFor(dto => dto.Instagram).MaximumLength(50);
        RuleFor(dto => dto.Github).MaximumLength(50);
        RuleFor(dto => dto.Medium).MaximumLength(50);
        RuleFor(dto => dto.Twitter).MaximumLength(50);
    }
}

public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(dto => dto.Username).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Password).NotEmpty().MinimumLength(5);
    }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(dto => dto.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.LastName).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Email).NotEmpty().EmailAddress();
        RuleFor(dto => dto.Phone).NotEmpty().MaximumLength(15);
        RuleFor(dto => dto.Username).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Website).MaximumLength(50);
        RuleFor(dto => dto.Facebook).MaximumLength(50);
        RuleFor(dto => dto.Instagram).MaximumLength(50);
        RuleFor(dto => dto.Github).MaximumLength(50);
        RuleFor(dto => dto.Medium).MaximumLength(50);
        RuleFor(dto => dto.Twitter).MaximumLength(50);
    }
}

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.LastName).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Email).NotEmpty().EmailAddress();
        RuleFor(dto => dto.Phone).NotEmpty().MaximumLength(15);
        RuleFor(dto => dto.Username).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Facebook).MaximumLength(50);
        RuleFor(dto => dto.Instagram).MaximumLength(50);
        RuleFor(dto => dto.Github).MaximumLength(50);
        RuleFor(dto => dto.Medium).MaximumLength(50);
        RuleFor(dto => dto.Twitter).MaximumLength(50);
    }
}