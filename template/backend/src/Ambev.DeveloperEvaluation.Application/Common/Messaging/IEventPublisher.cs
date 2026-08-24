namespace Ambev.DeveloperEvaluation.Application.Common.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(string topic, TEvent @event, CancellationToken cancellationToken = default);
}
