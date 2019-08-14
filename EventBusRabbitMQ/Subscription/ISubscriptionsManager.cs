using EventBusRabbitMQ.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusRabbitMQ.Subscription
{
    public interface ISubscriptionsManager
    {
        string GetEventKey<T>();

        void AddSubscription<T, TH>()
           where T : IntegrationEvent
           where TH : IIntegrationEventHandler<T>;

        bool HasSubscriptionsForEvent(string eventName);


    }
}
