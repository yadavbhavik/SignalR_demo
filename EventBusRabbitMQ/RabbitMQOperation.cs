using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusRabbitMQ
{
    class RabbitMQOperation
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
    }
}
