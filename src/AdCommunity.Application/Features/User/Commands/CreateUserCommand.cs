using AdCommunity.Application.DTOs.User;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.User.Commands;

public class CreateUserCommand : IYtRequest<UserCreateDto>
{
    public UserCreateDto User { get; set; }
}

public class CreateUserCommandHandler : IYtRequestHandler<CreateUserCommand, UserCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserCreateDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.UserRepository.GetUsersByUsernameAndPasswordAsync(request.User.Username,request.User.Password);

        if (existingUser.Any())
        {
            throw new Exception("User already exists");
        }

        var user = new AdCommunity.Domain.Entities.Aggregates.User.User
        (request.User.FirstName, request.User.LastName, request.User.Email, request.User.Password, request.User.Phone, request.User.Username, request.User.Website, request.User.Facebook, request.User.Twitter, request.User.Instagram, request.User.Github, request.User.Medium);

        await _unitOfWork.UserRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<AdCommunity.Domain.Entities.Aggregates.User.User, UserCreateDto>(user);
    }
}
