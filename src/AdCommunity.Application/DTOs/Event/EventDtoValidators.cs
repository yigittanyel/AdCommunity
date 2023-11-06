using FluentValidation;

namespace AdCommunity.Application.DTOs.Event;

public class EventCreateDtoValidator:AbstractValidator<EventCreateDto>
{
    public EventCreateDtoValidator()
    {
        RuleFor(dto => dto.EventName).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.Description).NotEmpty().MaximumLength(500);
        RuleFor(dto => dto.EventDate).NotEmpty();
        RuleFor(dto => dto.Location).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.CommunityId).GreaterThan(0).NotNull();
    }
}

public class EventUpdateDtoValidator : AbstractValidator<EventUpdateDto>
{
    public EventUpdateDtoValidator()
    {
        RuleFor(dto => dto.Id).GreaterThan(0).NotNull();
        RuleFor(dto => dto.EventName).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.Description).NotEmpty().MaximumLength(500);
        RuleFor(dto => dto.EventDate).NotEmpty();
        RuleFor(dto => dto.Location).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.CommunityId).GreaterThan(0).NotNull();
    }
}

public class EventDtoValidator : AbstractValidator<EventDto>
{
    public EventDtoValidator()
    {
        RuleFor(dto => dto.Id).GreaterThan(0).NotNull();
        RuleFor(dto => dto.EventName).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.Description).NotEmpty().MaximumLength(500);
        RuleFor(dto => dto.EventDate).NotEmpty();
        RuleFor(dto => dto.Location).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.CommunityId).GreaterThan(0).NotNull();
    }
}