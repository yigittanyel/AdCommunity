using AdCommunity.Application.DTOs.UserEvent;
using AdCommunity.Application.Features.UserEvent.Commands.CreateUserEventCommand;
using AdCommunity.Application.Features.UserEvent.Commands.UpdateUserEventCommand;
using FluentValidation;

namespace AdCommunity.Application.Validators;

public class UserEventDtoValidator: AbstractValidator<UserEventDto>
{
    public UserEventDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.UserId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.EventId).NotEmpty().GreaterThan(0);
    }
}

public class CreateUserEventCommandValidator: AbstractValidator<CreateUserEventCommand>
{
    public CreateUserEventCommandValidator()
    {
        RuleFor(dto => dto.UserId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.EventId).NotEmpty().GreaterThan(0);
    }
}

public class UpdateUserEventCommandValidator: AbstractValidator<UpdateUserEventCommand>
{
    public UpdateUserEventCommandValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.UserId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.EventId).NotEmpty().GreaterThan(0);
    }
}
