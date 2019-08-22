using EventBusRabbitMQ.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hub.Events
{
    public class OrderbookDataChangeIntegrationEventHandler : IIntegrationEventHandler<OrderbookDataChangeIntegrationEvent>
    {
        private readonly INotificationHub notificationHub;

        public OrderbookDataChangeIntegrationEventHandler(INotificationHub notificationHub)
        {
            this.notificationHub = notificationHub;
        }

        public Task Handle(OrderbookDataChangeIntegrationEvent @event)
        {
            //TODO: redis cache retrive logic akash
            //SignalR hub method call template
            string Data = null; // data are retrieved from redis
            if (@event._LpName.Equals("BuyerBookLP"))
            {
                notificationHub.BuyerBookLP(@event._LpName, @event._PairName, Data);
                return Task.FromResult(0);
            }

            if (@event._LpName.Equals("SellerBookLP"))
            {
                notificationHub.SellerBookLP(@event._LpName, @event._PairName, Data);
                return Task.FromResult(0);
            }

            throw new ArgumentException("{0} is not valid _LpName", @event._LpName);

        }
    }
}
