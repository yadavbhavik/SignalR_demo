using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusRabbitMQ
{
    class DefaultRabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private IConnectionFactory _connectionFactory;
        IConnection _connection;

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen;
            }
        }

        public DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public bool TryConnect()
        {
            _connection = _connectionFactory
                          .CreateConnection();

            if (IsConnected)
            {
                return true;
            }

            return false;
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }
    }
}
