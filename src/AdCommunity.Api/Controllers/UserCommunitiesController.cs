using AdCommunity.Application.DTOs.UserCommunity;
using AdCommunity.Application.Features.UserCommunity.Commands;
using AdCommunity.Application.Features.UserCommunity.Queries;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCommunitiesController : ControllerBase
    {
        private readonly IYtMediator _mediator;

        public UserCommunitiesController(IYtMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]/{userCommunityId}")]
        public async Task<UserCommunityDto> Get(int userCommunityId)
        {
            GetUserCommunityQuery query = new GetUserCommunityQuery { Id = userCommunityId };
            UserCommunityDto userCommunity = await _mediator.Send(query);
            return userCommunity;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<UserCommunityDto>> GetAll()
        {
            GetUserCommunitiesQuery query = new GetUserCommunitiesQuery();
            IEnumerable<UserCommunityDto> userCommunities = await _mediator.Send(query);

            return userCommunities;
        }

        [HttpPost("[action]")]
        public async Task<UserCommunityCreateDto> Create(UserCommunityCreateDto userCommunity)
        {
            CreateUserCommunityCommand command = new CreateUserCommunityCommand(userCommunity.UserId, userCommunity.CommunityId, userCommunity.JoinDate);
            UserCommunityCreateDto createdUserCommunity = await _mediator.Send(command);

            return createdUserCommunity;
        }

        [HttpPut("[action]")]
        public async Task<bool> Update(UserCommunityUpdateDto userCommunity)
        {
            UpdateUserCommunityCommand command = new UpdateUserCommunityCommand(userCommunity.Id,userCommunity.UserId, userCommunity.CommunityId, userCommunity.JoinDate);
            bool updatedUserCommunity = await _mediator.Send(command);

            return updatedUserCommunity;
        }

        [HttpDelete("[action]/{userCommunityId}")]
        public async Task<bool> Delete(int userCommunityId)
        {
            DeleteUserCommunityCommand command = new DeleteUserCommunityCommand { Id = userCommunityId };
            bool deletedUserCommunity = await _mediator.Send(command);

            return deletedUserCommunity;
        }
    }
}
