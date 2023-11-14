using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Features.Event.Commands.CreateEventCommand;
using AdCommunity.Application.Features.Event.Commands.DeleteEventCommand;
using AdCommunity.Application.Features.Event.Commands.UpdateEventCommand;
using AdCommunity.Application.Features.Event.Queries.GetEventQuery;
using AdCommunity.Application.Features.Event.Queries.GetEventsQuery;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IYtMediator _mediator;

        public EventsController(IYtMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]/{eventId}")]
        public async Task<EventDto> Get(int eventId)
        {
            GetEventQuery query = new GetEventQuery { Id = eventId };
            EventDto _event = await _mediator.Send(query);

            return _event;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<EventDto>> GetAll()
        {
            GetEventsQuery query = new GetEventsQuery();
            IEnumerable<EventDto> events = await _mediator.Send(query);

            return events;
        }

        [HttpPost("[action]")]
        public async Task<EventCreateDto> Create(EventCreateDto _event)
        {
            CreateEventCommand command = new CreateEventCommand(_event.EventName,_event.Description,_event.EventDate,_event.Location);
            EventCreateDto createdEvent = await _mediator.Send(command);

            return createdEvent;
        }

        [HttpPut("[action]")]
        public async Task<bool> Update(EventUpdateDto _event)
        {
            UpdateEventCommand command = new UpdateEventCommand(_event.Id,_event.EventName, _event.Description, _event.EventDate, _event.Location);
            bool updatedEvent = await _mediator.Send(command);

            return updatedEvent;
        }

        [HttpDelete("[action]/{eventId}")]
        public async Task<bool> Delete(int eventId)
        {
            DeleteEventCommand command = new DeleteEventCommand { Id = eventId };
            bool deletedEvent = await _mediator.Send(command);

            return deletedEvent;
        }
    }
}
