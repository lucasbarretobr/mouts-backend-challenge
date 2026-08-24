using System.Text.Json;
using Ambev.DeveloperEvaluation.Application.Common.Messaging;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.IoC.Messaging;

public sealed class KafkaEventPublisher : IEventPublisher, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly ILogger<KafkaEventPublisher> _logger;

    public KafkaEventPublisher(KafkaOptions options, ILogger<KafkaEventPublisher> logger)
    {
        _logger = logger;
        _producer = new ProducerBuilder<string, string>(new ProducerConfig
        {
            BootstrapServers = options.BootstrapServers,
            ClientId = options.ClientId,
            Acks = Acks.All
        }).Build();
    }

    public async Task PublishAsync<TEvent>(
        string topic,
        TEvent @event,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(topic);
        ArgumentNullException.ThrowIfNull(@event);

        var message = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString("N"),
            Value = JsonSerializer.Serialize(@event)
        };

        var result = await _producer.ProduceAsync(topic, message, cancellationToken);
        _logger.LogDebug(
            "Published event to Kafka topic {Topic}, partition {Partition}, offset {Offset}",
            topic,
            result.Partition,
            result.Offset);
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
}
