using MediatR;

namespace SenseTowerEventAPI.Interfaces;
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
