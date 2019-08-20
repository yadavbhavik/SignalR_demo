using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventBusRabbitMQ.Events;

namespace EventBusRabbitMQ.Subscription
{
    public class InMemorySubscriptionsManager : ISubscriptionsManager
    {

        private readonly Dictionary<string, List<SubscriptionInfo>> handlers;
        private readonly List<Type> eventTypes;

        public InMemorySubscriptionsManager()
        {
            handlers = new Dictionary<string, List<SubscriptionInfo>>();
            eventTypes = new List<Type>();
        }

        public void AddSubscription<T, TH>()
             where T : IntegrationEvent
             where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();

            DoAddSubscription(typeof(TH), eventName);

            if (!eventTypes.Contains(typeof(T)))
            {
                eventTypes.Add(typeof(T));
            }

        }

        private void DoAddSubscription(Type handlerType, string eventName)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                handlers.Add(eventName, new List<SubscriptionInfo>());
            }

            if (handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
        }

        public string GetEventKey<T>()
        {
            return typeof(T).Name;
        }

        public bool HasSubscriptionsForEvent(string eventName)
        {
            return handlers.ContainsKey(eventName);
        }

        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => handlers[eventName];

        public Type GetEventTypeByName(string eventName) => eventTypes.SingleOrDefault(t => t.Name == eventName);

        //Add public wrapper methods or testing private members -Sahil 19-08-2019
        public int EventTypeCountTest() => eventTypes.Count();
        public void DoAddSubscriptionTest(Type handlerName, string eventName) => DoAddSubscription(handlerName, eventName);
        
    }
}
