using EventBusRabbitMQ.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hub.Events
{
    public class TickerDataChangeIntegrationEventHandler : IIntegrationEventHandler<TickerDataChangeIntegrationEvent>
    {
        private readonly INotificationHub notificationHub;

        public TickerDataChangeIntegrationEventHandler(INotificationHub notificationHub)
        {
            this.notificationHub = notificationHub;
        }
        public Task Handle(TickerDataChangeIntegrationEvent @event)
        {
            //send received data from rabbit directly to the signalR -Sahil 22-08-2019
            notificationHub.TickerData(@event);

            return Task.FromResult(0);
        }
    }
}
