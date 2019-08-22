using EventBusRabbitMQ.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hub.Events.espay_layout_beta
{
    public class BookDataNotificationEvent: IntegrationEvent
    {
        /*example of members -Sahil 21-08-2019
         * Type e.x BuyerBookLP
         * Pair e.x like as DefaultPair or DefaultBaseCurrency from appsettings
         * Data e.x OrderBook data
         */
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
