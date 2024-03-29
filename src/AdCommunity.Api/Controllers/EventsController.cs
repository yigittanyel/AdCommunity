﻿using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Features.Event.Commands.CreateEventCommand;
using AdCommunity.Application.Features.Event.Commands.DeleteEventCommand;
using AdCommunity.Application.Features.Event.Commands.UpdateEventCommand;
using AdCommunity.Application.Features.Event.Queries.GetEventQuery;
using AdCommunity.Application.Features.Event.Queries.GetEventsQuery;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Authorize]
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
        public async Task<EventDto> Get(int eventId, CancellationToken cancellationToken)
        {
            GetEventQuery query = new GetEventQuery { Id = eventId };
            EventDto _event = await _mediator.Send(query,cancellationToken);

            return _event;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<EventDto>> GetAll(CancellationToken cancellationToken)
        {
            GetEventsQuery query = new GetEventsQuery();
            IEnumerable<EventDto> events = await _mediator.Send(query,cancellationToken);

            return events;
        }

        [HttpPost("[action]")]
        public async Task<EventCreateDto> Create(EventCreateDto @event, CancellationToken cancellationToken)
        {
            CreateEventCommand command = new CreateEventCommand(@event.EventName, @event.Description, @event.EventDate, @event.Location, @event.CommunityId);
            EventCreateDto createdEvent = await _mediator.Send(command,cancellationToken);

            return createdEvent;
        }

        [HttpPut("[action]")]
        public async Task<bool> Update(EventUpdateDto @event, CancellationToken cancellationToken)
        {
            UpdateEventCommand command = new UpdateEventCommand(@event.Id, @event.EventName, @event.Description, @event.EventDate, @event.Location, @event.CommunityId);
            bool updatedEvent = await _mediator.Send(command,cancellationToken);

            return updatedEvent;
        }

        [HttpDelete("[action]/{eventId}")]
        public async Task<bool> Delete(int eventId, CancellationToken cancellationToken)
        {
            DeleteEventCommand command = new DeleteEventCommand { Id = eventId };
            bool deletedEvent = await _mediator.Send(command,cancellationToken);

            return deletedEvent;
        }
    }
}
