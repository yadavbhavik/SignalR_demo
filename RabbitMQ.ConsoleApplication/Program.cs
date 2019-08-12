using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ.ConsoleApplication
{
    //Add rabbitMQ recevier code by bhavik yadav Date:12/08/19
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

                    Console.WriteLine(" [*] Waiting for logs.");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] {0}", message);
                    };
                    channel.BasicConsume(queue: "signalr-global",
                                         autoAck: true,
                                         consumer: consumer);

                }
            }

        }

    }
}
