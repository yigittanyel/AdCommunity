using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Features.Community.Commands;
using AdCommunity.Application.Features.Community.Queries;
using AdCommunity.Application.Features.User.Commands;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunitiesController : ControllerBase
    {
        private readonly IYtMediator _mediator;

        public CommunitiesController(IYtMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]/{communityId}")]
        public async Task<IActionResult> Get(int communityId)
        {
            GetCommunityQuery query = new GetCommunityQuery { Id = communityId };
            CommunityDto community = await _mediator.Send(query);

            if (community == null)
            {
                return NotFound();
            }

            return Ok(community);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            GetCommunitiesQuery query = new GetCommunitiesQuery();
            IEnumerable<CommunityDto> communities = await _mediator.Send(query);

            if (communities == null)
            {
                return NotFound();
            }

            return Ok(communities);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CommunityCreateDto community)
        {
            CreateCommunityCommand command = new CreateCommunityCommand { Community = community};
            CommunityCreateDto createdCommunity = await _mediator.Send(command);

            if (createdCommunity == null)
            {
                return BadRequest();
            }

            return Ok(createdCommunity);
        }

        [HttpDelete("[action]/{communityId}")]
        public async Task<IActionResult> Delete(int communityId)
        {
            DeleteCommunityCommand command = new DeleteCommunityCommand { Id = communityId };
            bool deletedCommunity = await _mediator.Send(command);

            if (!deletedCommunity)
            {
                return BadRequest();
            }

            return Ok(deletedCommunity);
        }


        [HttpPut("[action]")]
        public async Task<IActionResult> Update(CommunityUpdateDto community)
        {
            UpdateCommunityCommand command = new UpdateCommunityCommand { Community = community};
            bool updatedCommunity = await _mediator.Send(command);

            if (!updatedCommunity)
            {
                return BadRequest();
            }

            return Ok(updatedCommunity);
        }
    }
}
