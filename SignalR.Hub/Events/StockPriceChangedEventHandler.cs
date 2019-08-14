using EventBusRabbitMQ.Events;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hub.Events
{
    public class StockPriceChangedEventHandler : EventBusRabbitMQ.Events.IIntegrationEventHandler<StockPriceChangedEvent>
    {
        private readonly IHubContext<NotificationHub> hubContext;

        public StockPriceChangedEventHandler(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public Task Handle(StockPriceChangedEvent @event)
        {
            hubContext.Clients.All.SendAsync("UpdatedOrderState", @event.message);
            return Task.FromResult(true);
        }
    }
}
