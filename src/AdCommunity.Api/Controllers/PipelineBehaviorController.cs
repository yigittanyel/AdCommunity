using AdCommunity.Application.Features.PipelineExample;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PipelineBehaviorController : ControllerBase
    {
        private readonly IYtMediator _mediator;

        public PipelineBehaviorController(IYtMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<string> Example()
        {
            try
            {
                ExampleReq request = new ExampleReq { Message = "Hello, World!" };
                var response = await _mediator.Send(request);
                return response;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
