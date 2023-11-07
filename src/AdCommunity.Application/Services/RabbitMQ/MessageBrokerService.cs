using RabbitMQ.Client;
using System.Text;

namespace AdCommunity.Application.Services.RabbitMQ;

public class MessageBrokerService : IMessageBrokerService
{
    private readonly ConnectionFactory factory;

    public MessageBrokerService(ConnectionFactory factory)
    {
        this.factory = factory;
    }

    public void PublishMessage(string queueName, string message)
    {
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
    }
}
