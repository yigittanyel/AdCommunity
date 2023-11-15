using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Features.Event.Commands.CreateEventCommand;
using AdCommunity.Application.Features.Event.Commands.UpdateEventCommand;
using FluentValidation;

namespace AdCommunity.Application.Validators;

public class EventDtoValidator : AbstractValidator<EventDto>
{
    public EventDtoValidator()
    {
        RuleFor(dto => dto.Id).GreaterThan(0).NotNull();
        RuleFor(dto => dto.EventName).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.Description).NotEmpty().MaximumLength(500);
        RuleFor(dto => dto.EventDate).NotEmpty();
        RuleFor(dto => dto.Location).NotEmpty().MaximumLength(50);
    }
}

public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(dto => dto.EventName).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.Description).NotEmpty().MaximumLength(500);
        RuleFor(dto => dto.EventDate).NotEmpty();
        RuleFor(dto => dto.Location).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.CommunityId).GreaterThan(0).NotNull();
    }
}

public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(dto => dto.Id).GreaterThan(0).NotNull();
        RuleFor(dto => dto.EventName).NotEmpty().MaximumLength(100);
        RuleFor(dto => dto.Description).NotEmpty().MaximumLength(500);
        RuleFor(dto => dto.EventDate).NotEmpty();
        RuleFor(dto => dto.Location).NotEmpty().MaximumLength(50);
        RuleFor(dto => dto.CommunityId).GreaterThan(0).NotNull();
    }
}
