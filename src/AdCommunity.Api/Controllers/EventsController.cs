using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Features.Event.Commands;
using AdCommunity.Application.Features.Event.Queries;
using AdCommunity.Application.Features.User.Commands;
using AdCommunity.Application.Features.User.Queries;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Get(int eventId)
        {
            GetEventQuery query = new GetEventQuery { Id = eventId };
            EventDto _event = await _mediator.Send(query);

            if (_event == null)
            {
                return NotFound();
            }

            return Ok(_event);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            GetEventsQuery query = new GetEventsQuery();
            IEnumerable<EventDto> events = await _mediator.Send(query);

            if (events == null)
            {
                return NotFound();
            }

            return Ok(events);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(EventCreateDto _event)
        {
            CreateEventCommand command = new CreateEventCommand { Event = _event };
            EventCreateDto createdEvent = await _mediator.Send(command);

            if (createdEvent == null)
            {
                return BadRequest();
            }

            return Ok(createdEvent);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update(EventUpdateDto _event)
        {
            UpdateEventCommand command = new UpdateEventCommand { Event = _event };
            bool updatedEvent = await _mediator.Send(command);

            if (!updatedEvent)
            {
                return BadRequest();
            }

            return Ok(updatedEvent);
        }

        [HttpDelete("[action]/{eventId}")]
        public async Task<IActionResult> Delete(int eventId)
        {
            DeleteEventCommand command = new DeleteEventCommand { Id = eventId };
            bool deletedEvent = await _mediator.Send(command);

            if (!deletedEvent)
            {
                return BadRequest();
            }

            return Ok(deletedEvent);
        }
    }
}
