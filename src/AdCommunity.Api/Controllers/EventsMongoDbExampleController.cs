using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Services.MongoDB;
using AdCommunity.Domain.Entities.Aggregates.Community;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsMongoDbExampleController : ControllerBase
    {
        private readonly IMongoDbService<EventMongo> _mongoDbService;

        public EventsMongoDbExampleController(IMongoDbService<EventMongo> mongoDbService)
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
            var result = await _mongoDbService.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(EventCreateDto @event)
        {
            var eventEntity = new EventMongo(
                @event.EventName,
                @event.Description,
                @event.EventDate,
                @event.Location
            );

            await _mongoDbService.CreateAsync(eventEntity);

            return Ok(eventEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, EventUpdateDto entity)
        {
            var @event = new EventMongo(entity.EventName, entity.Description, entity.EventDate, entity.Location);
            @event.Id = id;
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
