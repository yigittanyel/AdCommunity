using AdCommunity.Application.DTOs.UserTicket;
using AdCommunity.Application.Features.UserTicket.Commands.CreateUserTicketCommand;
using AdCommunity.Application.Features.UserTicket.Commands.UpdateUserTicketCommand;
using FluentValidation;

namespace AdCommunity.Application.Validators;

public class UserTicketDtoValidator : AbstractValidator<UserTicketDto>
{
    public UserTicketDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.UserId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.TicketId).NotEmpty().GreaterThan(0);
    }
}

public class CreateUserTicketCommandValidator : AbstractValidator<CreateUserTicketCommand>
{
    public CreateUserTicketCommandValidator()
    {
        RuleFor(dto => dto.UserId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.TicketId).NotEmpty().GreaterThan(0);
    }
}

public class UpdateUserTicketCommandValidator : AbstractValidator<UpdateUserTicketCommand>
{
    public UpdateUserTicketCommandValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.UserId).NotEmpty().GreaterThan(0);
        RuleFor(dto => dto.TicketId).NotEmpty().GreaterThan(0);
    }
}


