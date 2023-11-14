using AdCommunity.Application.DTOs.UserTicket;
using AdCommunity.Application.Features.UserTicket.Commands.CreateUserTicketCommand;
using AdCommunity.Application.Features.UserTicket.Commands.DeleteUserTicketCommand;
using AdCommunity.Application.Features.UserTicket.Commands.UpdateUserTicketCommand;
using AdCommunity.Application.Features.UserTicket.Queries.GetUserTicketQuery;
using AdCommunity.Application.Features.UserTicket.Queries.GetUserTicketsQuery;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTicketsController : ControllerBase
    {
        private readonly IYtMediator _mediator;

        public UserTicketsController(IYtMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]/{userTicketId}")]
        public async Task<UserTicketDto> Get(int userTicketId)
        {
            GetUserTicketQuery query = new GetUserTicketQuery { Id = userTicketId };
            UserTicketDto userTicket = await _mediator.Send(query);
            return userTicket;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<UserTicketDto>> GetAll()
        {
            GetUserTicketsQuery query = new GetUserTicketsQuery();
            IEnumerable<UserTicketDto> userTickets = await _mediator.Send(query);

            return userTickets;
        }

        [HttpPost("[action]")]
        public async Task<UserTicketCreateDto> Create(UserTicketCreateDto userTicket)
        {
            CreateUserTicketCommand command = new CreateUserTicketCommand(userTicket.UserId,userTicket.TicketId, userTicket.Pnr);

            UserTicketCreateDto createdUserTicket = await _mediator.Send(command);

            return createdUserTicket;
        }

        [HttpPut("[action]")]
        public async Task<bool> Update(UserTicketUpdateDto userTicket)
        {
            UpdateUserTicketCommand command = new UpdateUserTicketCommand(userTicket.Id,userTicket.UserId,userTicket.TicketId,userTicket.Pnr);
            bool updatedUserTicket = await _mediator.Send(command);

            return updatedUserTicket;
        }

        [HttpDelete("[action]/{userTicketId}")]
        public async Task<bool> Delete(int userTicketId)
        {
            DeleteUserTicketCommand command = new DeleteUserTicketCommand { Id = userTicketId };
            bool deletedUserTicket = await _mediator.Send(command);

            return deletedUserTicket;
        }
    }
}
