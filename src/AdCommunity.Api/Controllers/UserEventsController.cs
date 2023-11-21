using AdCommunity.Application.DTOs.UserEvent;
using AdCommunity.Application.Features.UserEvent.Commands.CreateUserEventCommand;
using AdCommunity.Application.Features.UserEvent.Commands.DeleteUserEventCommand;
using AdCommunity.Application.Features.UserEvent.Commands.UpdateUserEventCommand;
using AdCommunity.Application.Features.UserEvent.Queries.GetUserEventQuery;
using AdCommunity.Application.Features.UserEvent.Queries.GetUserEventsQuery;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserEventsController : ControllerBase
    {
        private readonly IYtMediator _mediator;

        public UserEventsController(IYtMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]/{userEventId}")]
        public async Task<UserEventDto> Get(int userEventId)
        {
            GetUserEventQuery query = new GetUserEventQuery { Id = userEventId };
            UserEventDto userEvent = await _mediator.Send(query);
            return userEvent;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<UserEventDto>> GetAll()
        {
            GetUserEventsQuery query = new GetUserEventsQuery();
            IEnumerable<UserEventDto> userEvents = await _mediator.Send(query);

            return userEvents;
        }

        [HttpPost("[action]")]
        public async Task<UserEventCreateDto> Create(UserEventCreateDto userEvent)
        {
            CreateUserEventCommand command = new CreateUserEventCommand(userEvent.UserId, userEvent.EventId);
            UserEventCreateDto createdUserEvent = await _mediator.Send(command);

            return createdUserEvent;
        }

        [HttpPut("[action]")]
        public async Task<bool> Update(UserEventUpdateDto userEvent)
        {
            UpdateUserEventCommand command = new UpdateUserEventCommand(userEvent.Id, userEvent.UserId, userEvent.EventId);
            bool updatedUserEvent = await _mediator.Send(command);

            return updatedUserEvent;
        }

        [HttpDelete("[action]/{userEventId}")]
        public async Task<bool> Delete(int userEventId)
        {
            DeleteUserEventCommand command = new DeleteUserEventCommand { Id = userEventId };
            bool deletedUserEvent = await _mediator.Send(command);

            return deletedUserEvent;
        }
    }
}
