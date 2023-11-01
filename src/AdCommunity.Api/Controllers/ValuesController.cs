using AdCommunity.Application.Features.User.Queries;
using AdCommunity.Core.Extensions.Mediator;
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
        private readonly IYtMediator _ytMediator;

        public ValuesController(IYtMediator ytMediator)
        {
            _ytMediator = ytMediator;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var query=new GetUsersQuery();
            return Ok(_ytMediator.Send(query));
        }
    }
}
