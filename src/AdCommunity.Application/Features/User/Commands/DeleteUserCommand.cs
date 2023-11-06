using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.User.Commands;

public class DeleteUserCommand : IYtRequest<bool>
{
    public int Id { get; set; }
}

public class DeleteUserCommandHandler : IYtRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.UserRepository.GetAsync(request.Id, cancellationToken);

        if (existingUser == null)
        {
            throw new Exception("User does not exist");
        }

        _unitOfWork.UserRepository.Delete(existingUser);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
