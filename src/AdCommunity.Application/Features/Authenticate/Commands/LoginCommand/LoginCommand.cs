using AdCommunity.Application.DTOs.User;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Entities.SharedKernel;

namespace AdCommunity.Application.Features.Authenticate.Commands.LoginCommand;

public class LoginCommand : IYtRequest<Tokens>
{
    public UserLoginDto User { get; set; }
}
