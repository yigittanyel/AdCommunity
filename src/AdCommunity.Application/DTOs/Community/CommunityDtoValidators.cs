using FluentValidation;

namespace AdCommunity.Application.DTOs.Community;

public class CommunityCreateDtoValidator:AbstractValidator<CommunityCreateDto>
{
    public CommunityCreateDtoValidator()
    {
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Description).MaximumLength(500);
        RuleFor(dto => dto.Location).MinimumLength(2).MaximumLength(100);
        RuleFor(dto => dto.Website).MaximumLength(50);
        RuleFor(dto => dto.Facebook).MaximumLength(50);
        RuleFor(dto => dto.Instagram).MaximumLength(50);
        RuleFor(dto => dto.Github).MaximumLength(50);
        RuleFor(dto => dto.Medium).MaximumLength(50);
        RuleFor(dto => dto.Twitter).MaximumLength(50);
    }
}

public class CommunityUpdateDtoValidator : AbstractValidator<CommunityUpdateDto>
{
    public CommunityUpdateDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Description).MaximumLength(500);
        RuleFor(dto => dto.Location).MinimumLength(2).MaximumLength(100);
        RuleFor(dto => dto.Website).MaximumLength(50);
        RuleFor(dto => dto.Facebook).MaximumLength(50);
        RuleFor(dto => dto.Instagram).MaximumLength(50);
        RuleFor(dto => dto.Github).MaximumLength(50);
        RuleFor(dto => dto.Medium).MaximumLength(50);
        RuleFor(dto => dto.Twitter).MaximumLength(50);
    }
}   

public class CommunityDtoValidator:AbstractValidator<CommunityDto>
{
    public CommunityDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.Name).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.Description).MaximumLength(500);
        RuleFor(dto => dto.Location).MinimumLength(2).MaximumLength(100);
        RuleFor(dto => dto.Website).MaximumLength(50);
        RuleFor(dto => dto.Facebook).MaximumLength(50);
        RuleFor(dto => dto.Instagram).MaximumLength(50);
        RuleFor(dto => dto.Github).MaximumLength(50);
        RuleFor(dto => dto.Medium).MaximumLength(50);
        RuleFor(dto => dto.Twitter).MaximumLength(50);
    }
}

