using EventBusRabbitMQ.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hub.Events.espay_layout_beta
{
    public class BookDataNotificationEvent: IntegrationEvent
    {
        public string Type { get; }
        public string Pair { get; }
        public string Data { get; }

        public BookDataNotificationEvent(string type, string pair, string data)
        {
            Type = type;
            Pair = pair;
            Data = data;
        }
    }
}
