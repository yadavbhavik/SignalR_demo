using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusRabbitMQ
{
    class RabbitMQOperation : IRabbitMQOperation
    {
        private readonly IRabbitMQPersistentConnection persistentConnection;
        private readonly string queueName;
        private IModel consumerChannel;


        public RabbitMQOperation(IRabbitMQPersistentConnection persistentConnection, string queueName = null)
        {
            this.persistentConnection = persistentConnection;
            this.queueName = queueName;
            this.consumerChannel = CreateConsumerChannel();
        }



        private IModel CreateConsumerChannel()
        {
            //check of rabbitmq connection and connect if connection is not exist -Sahil 12-08-2019
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }


            var channel = persistentConnection.CreateModel();
            return channel;
        }

        // add retriveMessage method by bhavik yadav date:12/08/19
        public string RetriveMessage()
        {
            var channel = consumerChannel;
            var message ="";
            channel.ExchangeDeclare(exchange: "client-update", type: "direct", true);

            // var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: "client-update",
                              routingKey: "notification");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
               message = Encoding.UTF8.GetString(body);
              //  Console.WriteLine(" [x] {0}", message);
            };
            channel.BasicConsume(queue:queueName,
                                 autoAck: true,
                                 consumer: consumer);
            return message;
        }

    }
}
