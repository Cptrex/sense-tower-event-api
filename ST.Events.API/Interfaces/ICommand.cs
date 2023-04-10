using MediatR;

namespace ST.Events.API.Interfaces;
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
