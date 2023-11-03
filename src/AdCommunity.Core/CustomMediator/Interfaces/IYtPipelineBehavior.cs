﻿namespace AdCommunity.Core.CustomMediator.Interfaces;

public interface IYtPipelineBehavior<in TRequest, TResponse> where TRequest : IYtRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
}