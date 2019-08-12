using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalR.Hub.Events
{
    public class NotificationEventHandler : INotificationHandler<NotificationEvent>
    {
        private readonly IHubContext<NotificationHub> hubContext;

        public NotificationEventHandler(IHubContext<NotificationHub> hubContext)
        {
            this.hubContext = hubContext;
        }
        public Task Handle(NotificationEvent notification, CancellationToken cancellationToken)
        {
            //send notification to the client using signalR -Sahil 12-08-2019
            hubContext.Clients.All.SendAsync(notification.Message);
            return Task.FromResult(true);
        }
    }
}
