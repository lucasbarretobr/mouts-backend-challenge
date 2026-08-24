namespace Ambev.DeveloperEvaluation.IoC.Messaging;

public sealed class KafkaOptions
{
    public string BootstrapServers { get; set; } = "localhost:9092";
    public string ClientId { get; set; } = "ambev-developer-evaluation";
}
