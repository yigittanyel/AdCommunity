using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Features.Event.Commands.CreateEventCommand;
using AdCommunity.Application.Services.MongoDB;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Entities.Aggregates.Community;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Nest;
using System.Reflection;
using System.Threading;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsMongoDbExampleController : ControllerBase
    {
        private readonly IMongoDbService<Event> _mongoDbService;

        public EventsMongoDbExampleController(IMongoDbService<Event> mongoDbService, IYtMediator mediator)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _mongoDbService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            return Ok(await _mongoDbService.GetByIdAsync(id));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(EventCreateDto @event, CancellationToken cancellationToken)
        {
            var eventEntity = new Event(
                @event.EventName,
                @event.Description,
                @event.EventDate,
                @event.Location
            );

            eventEntity.Id = Convert.ToInt32(ObjectId.GenerateNewId());

            await _mongoDbService.CreateAsync(eventEntity);

            return Ok(eventEntity);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, EventUpdateDto entity)
        {
            var @event = new Event(entity.EventName, entity.Description, entity.EventDate, entity.Location);
            await _mongoDbService.UpdateAsync(id, @event);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _mongoDbService.DeleteAsync(id);
            return Ok();
        }
    }
}
