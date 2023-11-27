//using AdCommunity.Core.CustomMediator.Interfaces;
//using AdCommunity.Domain.Repository;

//namespace AdCommunity.Application.Transaction;

//public class TransactionalRequestHandlerDecorator<TRequest, TResponse> : IYtRequestHandler<TRequest, TResponse>
//        where TRequest : IYtRequest<TResponse>
//{
//    private readonly IYtRequestHandler<TRequest, TResponse> _innerHandler;
//    private readonly IUnitOfWork _unitOfWork;

//    public TransactionalRequestHandlerDecorator(IYtRequestHandler<TRequest, TResponse> innerHandler, IUnitOfWork unitOfWork)
//    {
//        _innerHandler = innerHandler;
//        _unitOfWork = unitOfWork;
//    }

//    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
//    {
//        // Transaction başlat
//        using (var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken))
//        {
//            try
//            {
//                // Inner handler'ı çağır
//                var response = await (_innerHandler as ITransactionalRequestHandler<TRequest, TResponse>)
//                    .HandleAsync(request, cancellationToken, _unitOfWork);

//                // Save changes
//                await _unitOfWork.SaveChangesAsync(cancellationToken);

//                // Transaction commit
//                await transaction.CommitAsync(cancellationToken);

//                // Response'u geri döndür
//                return response;
//            }
//            catch (Exception)
//            {
//                // Hata durumunda rollback yap
//                await transaction.RollbackAsync(cancellationToken);
//                throw;
//            }
//        }
//    }
//}