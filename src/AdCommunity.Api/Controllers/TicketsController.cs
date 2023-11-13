using AdCommunity.Application.DTOs.Ticket;
using AdCommunity.Application.Features.Ticket.Commands.CreateTicketCommand;
using AdCommunity.Application.Features.Ticket.Commands.DeleteTicketCommand;
using AdCommunity.Application.Features.Ticket.Commands.UpdateTicketCommand;
using AdCommunity.Application.Features.Ticket.Queries.GetTicketQuery;
using AdCommunity.Application.Features.Ticket.Queries.GetTicketsQuery;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IYtMediator _mediator;

        public TicketsController(IYtMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]/{ticketId}")]
        public async Task<TicketDto> Get(int ticketId)
        {
            GetTicketQuery query = new GetTicketQuery { Id = ticketId };
            TicketDto ticket = await _mediator.Send(query);

            return ticket;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<TicketDto>> GetAll()
        {
            GetTicketsQuery query = new GetTicketsQuery();
            IEnumerable<TicketDto> tickets = await _mediator.Send(query);

            return tickets;
        }

        [HttpPost("[action]")]
        public async Task<TicketCreateDto> Create(TicketCreateDto ticket)
        {
            CreateTicketCommand command = new CreateTicketCommand(ticket.CommunityEventId,ticket.CommunityId,ticket.Price);

            TicketCreateDto createdTicket = await _mediator.Send(command);

            return createdTicket;
        }

        [HttpDelete("[action]/{ticketId}")]
        public async Task<bool> Delete(int ticketId)
        {
            DeleteTicketCommand command = new DeleteTicketCommand { Id = ticketId };
            bool deletedTicket = await _mediator.Send(command);

            return deletedTicket;
        }


        [HttpPut("[action]")]
        public async Task<bool> Update(TicketUpdateDto ticket)
        {
            UpdateTicketCommand command = new UpdateTicketCommand(ticket.Id,ticket.CommunityEventId,ticket.CommunityId,ticket.Price);
            bool updatedTicket = await _mediator.Send(command);

            return updatedTicket;
        }
    }
}
