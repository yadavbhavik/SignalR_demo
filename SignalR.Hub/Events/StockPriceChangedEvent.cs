using EventBusRabbitMQ.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hub.Events
{
    public class StockPriceChangedEvent: IntegrationEvent
    {
        public string message { get; }

        public StockPriceChangedEvent(string message)
        {
            this.message = message;
        }
    }
}
