using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.Features.Community.Commands.CreateCommunityCommand;
using AdCommunity.Application.Features.Community.Commands.UpdateCommunityCommand;
using FluentValidation;

namespace AdCommunity.Application.Validators;

public class CommunityDtoValidator : AbstractValidator<CommunityDto>
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

public class CreateCommunityCommandValidator : AbstractValidator<CreateCommunityCommand>
{
    public CreateCommunityCommandValidator()
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

public class UpdateCommunityCommandValidator : AbstractValidator<UpdateCommunityCommand>
{
    public UpdateCommunityCommandValidator()
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
