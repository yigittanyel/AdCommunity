using AdCommunity.Application.DTOs.Ticket;
using FluentValidation;

namespace AdCommunity.Application.Validators;

public class TicketDtoValidator:AbstractValidator<TicketDto>
{
    public TicketDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.CommunityEventId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.CommunityId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.Price).NotEmpty().GreaterThan(0);
    }
}

public class CreateTicketCommandValidator:AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(dto => dto.CommunityEventId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.CommunityId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.Price).NotEmpty().GreaterThan(0);
    }
}

public class UpdateTicketCommandValidator:AbstractValidator<UpdateTicketCommand>
{
    public UpdateTicketCommandValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.CommunityEventId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.CommunityId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.Price).NotEmpty().GreaterThan(0);
    }
}


