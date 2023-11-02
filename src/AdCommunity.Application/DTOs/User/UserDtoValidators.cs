using FluentValidation;

namespace AdCommunity.Application.DTOs.User;

public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateDtoValidator()
    {
        RuleFor(dto => dto.Data.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Data.LastName).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Data.Email).NotEmpty().EmailAddress();
        RuleFor(dto => dto.Data.Password).NotEmpty().MinimumLength(6);
        RuleFor(dto => dto.Data.Phone).NotEmpty().MaximumLength(15);
        RuleFor(dto => dto.Data.Username).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Data.Facebook).MaximumLength(50);
        RuleFor(dto => dto.Data.Instagram).MaximumLength(50);
        RuleFor(dto => dto.Data.Github).MaximumLength(50);
        RuleFor(dto => dto.Data.Medium).MaximumLength(50);
        RuleFor(dto => dto.Data.Twitter).MaximumLength(50);     
    }
}

public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserUpdateDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.Data.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Data.LastName).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Data.Email).NotEmpty().EmailAddress();
        RuleFor(dto => dto.Data.Password).NotEmpty().MinimumLength(6);
        RuleFor(dto => dto.Data.Phone).NotEmpty().MaximumLength(15);
        RuleFor(dto => dto.Data.Username).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Data.Facebook).MaximumLength(50);
        RuleFor(dto => dto.Data.Instagram).MaximumLength(50);
        RuleFor(dto => dto.Data.Github).MaximumLength(50);
        RuleFor(dto => dto.Data.Medium).MaximumLength(50);
        RuleFor(dto => dto.Data.Twitter).MaximumLength(50);
    }
}

public class UserDtoValidator : AbstractValidator<UserUpdateDto>
{
    public UserDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.Data.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Data.LastName).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Data.Email).NotEmpty().EmailAddress();
        RuleFor(dto => dto.Data.Password).NotEmpty().MinimumLength(6);
        RuleFor(dto => dto.Data.Phone).NotEmpty().MaximumLength(15);
        RuleFor(dto => dto.Data.Username).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Data.Facebook).MaximumLength(50);
        RuleFor(dto => dto.Data.Instagram).MaximumLength(50);
        RuleFor(dto => dto.Data.Github).MaximumLength(50);
        RuleFor(dto => dto.Data.Medium).MaximumLength(50);
        RuleFor(dto => dto.Data.Twitter).MaximumLength(50);
    }
}

public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginDtoValidator()
    {
        RuleFor(dto => dto.Username).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Password).NotEmpty().MinimumLength(6);
    }
}