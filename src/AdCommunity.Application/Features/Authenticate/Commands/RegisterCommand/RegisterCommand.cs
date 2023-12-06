using AdCommunity.Application.DTOs.User;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Entities.SharedKernel;

namespace AdCommunity.Application.Features.Authenticate.Commands.RegisterCommand;

public class RegisterCommand : IYtRequest<Tokens>
{
    public UserCreateDto User { get; set; }
    public bool IsCommand => true;

}
