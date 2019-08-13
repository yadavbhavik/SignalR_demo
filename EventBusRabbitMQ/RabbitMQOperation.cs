using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Logging;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusRabbitMQ
{
    public class RabbitMQOperation : IRabbitMQOperation
    {
        private readonly IRabbitMQPersistentConnection persistentConnection;
        private readonly string queueName;
        private readonly IMediator mediator;
        private IModel consumerChannel;
        private string message;


        public RabbitMQOperation(IRabbitMQPersistentConnection persistentConnection, IMediator mediator, string queueName = null)
        {
            this.persistentConnection = persistentConnection;
            this.queueName = queueName;
            this.mediator = mediator;
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
            //var message = "";
            channel.ExchangeDeclare(exchange: "client-update", type: "direct", true);

            // var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: "client-update",
                              routingKey: "notification");

            var consumer = new EventingBasicConsumer(channel);
            try
            {
                NLogger.WriteLogIntoFile("RetriveMessage", "", "Outside consumer.Received ");
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body;
                    NLogger.WriteLogIntoFile("RetriveMessage", "", "Inside consumer.Received body: "+ body.ToString());

                    message += Encoding.UTF8.GetString(body);
                    NLogger.WriteLogIntoFile("RetriveMessage", "", "Inside consumer.Received message: "+ message);

                    //  Console.WriteLine(" [x] {0}", message);

                    //send event command to notification hub through mediatR -Sahil 12-08-2019
                    //await mediator.Publish(new NotificationEvent { Message = message });
                    
                };
            }
            catch (Exception e)
            {
                NLogger.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, "", e);
            }

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
            NLogger.WriteLogIntoFile(System.Reflection.MethodBase.GetCurrentMethod().Name, "", "After channel.BasicConsume message: " + message);
            return message;
        }

    }
}
