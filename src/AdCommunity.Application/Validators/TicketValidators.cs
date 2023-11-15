using AdCommunity.Application.DTOs.TicketTypes;
using AdCommunity.Application.Features.Ticket.Commands.CreateTicketCommand;
using AdCommunity.Application.Features.Ticket.Commands.UpdateTicketCommand;
using FluentValidation;

namespace AdCommunity.Application.Validators;

public class TicketDtoValidator:AbstractValidator<TicketTypesDto>
{
    public TicketDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.Price).NotEmpty().GreaterThan(0);
    }
}

public class CreateTicketCommandValidator:AbstractValidator<CreateTicketTypesCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(dto => dto.CommunityEventId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.CommunityId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.Price).NotEmpty().GreaterThan(0);
    }
}

public class UpdateTicketCommandValidator : AbstractValidator<UpdateTicketTypeCommand>
{
    public UpdateTicketCommandValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.CommunityEventId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.CommunityId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.Price).NotEmpty().GreaterThan(0);
    }
}


