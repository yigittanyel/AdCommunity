using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;

        public ValuesController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var x=_ticketRepository.QueryListAsync(selector:p=>new { p.Id,p.Price});
            return Ok(JsonSerializer.Serialize(x));
        }
    }
}
