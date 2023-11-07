using RabbitMQ.Client;

namespace AdCommunity.Application.Services.RabbitMQ;

public interface IMessageBrokerService
{
    void PublishMessage(string queueName, string message);
}
