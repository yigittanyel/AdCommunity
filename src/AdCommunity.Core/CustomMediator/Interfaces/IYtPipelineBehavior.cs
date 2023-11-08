namespace AdCommunity.Core.CustomMediator.Interfaces;

public interface IYtPipelineBehavior<in TRequest, TResponse> where TRequest : IYtRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}

#region PipelineBehavior
//Bu metotun amacı, bir işlemi gerçekleştirmeden önce ve sonra belirli davranışları eklemektir.
//Örneğin, bir işlemi başlatmadan önce loglama yapmak veya işlem sonuçlandıktan sonra belirli bir kontrolü yapmak için kullanılabilir.
#endregion