using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Console.RabbitMQ
{
    class Program
    {
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static QueueingBasicConsumer _consumer;

        private const string ExchangeName = "client-update";
        static void Main(string[] args)
        {
            _factory = new ConnectionFactory { HostName = "localhost", UserName = "guest", Password = "guest" };
            using (_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "client-update", type: "direct", true);

                    // var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: "signalr-global",
                                      exchange: "client-update",
                                      routingKey: "notification");

                    System.Console.WriteLine(" [*] Waiting for logs.");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        System.Console.WriteLine(" [x] {0}", message);
                    };
                    channel.BasicConsume(queue: "signalr-global",
                                         autoAck: true,
                                         consumer: consumer);

                }
            }

        }

    }
}
