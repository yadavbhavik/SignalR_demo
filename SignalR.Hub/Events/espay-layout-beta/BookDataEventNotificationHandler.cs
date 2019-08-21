using EventBusRabbitMQ.Events;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hub.Events.espay_layout_beta
{
    public class BookDataEventNotificationHandler : IIntegrationEventHandler<BookDataNotificationEvent>
    {
        private readonly IHubContext<NotificationHub> hubContext;

        public BookDataEventNotificationHandler(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public Task Handle(BookDataNotificationEvent @event)
        {
            //TODO: send received data -Sahil 21-08-2019
            //      To: the signalR groups connection client 
            //      From: event by RabbitMQ connsumer 
            //method e.x from espay-layout-beta project 
            //hubContext.Clients.Group(@event.Type + @event.Pair).SendAsync();
            return Task.CompletedTask;
        }
    }
}
