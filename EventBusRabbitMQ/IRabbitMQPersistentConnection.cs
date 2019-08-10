using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusRabbitMQ
{
    public interface IRabbitMQPersistentConnection
    {
        bool TryConnect();

        IModel CreateModel();

        bool IsConnected { get; }
    }
}
