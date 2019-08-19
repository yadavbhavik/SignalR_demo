using Autofac;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Logging;
using EventBusRabbitMQ.Subscription;
using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ
{
    public class RabbitMQOperation : IRabbitMQOperation
    {
        private readonly IRabbitMQPersistentConnection persistentConnection;
        private readonly string queueName;
        private readonly IMediator mediator;
        private readonly Subscription.ISubscriptionsManager subManager;
        private readonly ILifetimeScope lifetimeScope;
        private readonly string EXCHANGE_NAME = "client-update";
        private readonly string ROUTING_KEY = "notification";
        private IModel consumerChannel;
        private string message;


        public RabbitMQOperation(IRabbitMQPersistentConnection persistentConnection, ISubscriptionsManager subManager, ILifetimeScope lifetimeScope, string queueName = null)
        {
            this.persistentConnection = persistentConnection;
            this.queueName = queueName;
            //this.mediator = mediator;
            this.subManager = subManager;
            this.lifetimeScope = lifetimeScope;
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
            channel.ExchangeDeclare(exchange: EXCHANGE_NAME, type: "direct", true);

            channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var eventName = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(ea.Body);

                await ProcessEvent(eventName, message);

                channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            eventName = "StockPriceChangedEvent";
            if (subManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = subManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    var handler = lifetimeScope.ResolveOptional(subscription.HandlerType);
                    if (handler == null) continue;
                    var eventType = subManager.GetEventTypeByName(eventName);
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                }

            }
        }

        private void await(Task task)
        {
            throw new NotImplementedException();
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
                    NLogger.WriteLogIntoFile("RetriveMessage", "", "Inside consumer.Received body: " + body.ToString());

                    message += Encoding.UTF8.GetString(body);
                    NLogger.WriteLogIntoFile("RetriveMessage", "", "Inside consumer.Received message: " + message);

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

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = subManager.GetEventKey<T>();
            DoInternalSubscription(eventName);

            subManager.AddSubscription<T, TH>();

        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = subManager.HasSubscriptionsForEvent(eventName);

            if (!containsKey)
            {
                if (!persistentConnection.IsConnected)
                {
                    persistentConnection.TryConnect();
                }

                using (var channel = persistentConnection.CreateModel())
                {
                    channel.QueueBind(queue: queueName,
                                      exchange: EXCHANGE_NAME,
                                      routingKey: ROUTING_KEY);
                }
            }
        }
    }
}
