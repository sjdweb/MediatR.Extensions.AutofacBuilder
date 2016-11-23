using System.Threading.Tasks;

namespace MediatR
{
    internal class RequestHandlerWrapper<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _innerHandler;

        public RequestHandlerWrapper(IRequestHandler<TRequest, TResponse> innerHandler)
        {
            _innerHandler = innerHandler;
        }

        public TResponse Handle(TRequest message)
        {
            return _innerHandler.Handle(message);
        }
    }

    internal class AsyncRequestHandlerWrapper<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IAsyncRequest<TResponse>
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> _innerHandler;

        public AsyncRequestHandlerWrapper(IAsyncRequestHandler<TRequest, TResponse> innerHandler)
        {
            _innerHandler = innerHandler;
        }

        public async Task<TResponse> Handle(TRequest message)
        {
            return await _innerHandler.Handle(message);
        }
    }
}