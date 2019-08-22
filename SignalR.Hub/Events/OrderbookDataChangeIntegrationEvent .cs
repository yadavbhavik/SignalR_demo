using EventBusRabbitMQ.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hub.Events
{
    public class OrderbookDataChangeIntegrationEvent: IntegrationEvent
    {
        public string _LpName { get; set; }
        public string _PairName { get; set; }
        public string _OrderType { get; set; }

        public OrderbookDataChangeIntegrationEvent(string LpName, string PairName, string OrderType)
        {
            _LpName = LpName;
            _PairName = PairName;
            _OrderType = OrderType;
        }
    }
}
