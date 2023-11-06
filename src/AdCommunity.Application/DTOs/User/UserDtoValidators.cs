using FluentValidation;

namespace AdCommunity.Application.DTOs.User;

public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateDtoValidator()
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

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
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