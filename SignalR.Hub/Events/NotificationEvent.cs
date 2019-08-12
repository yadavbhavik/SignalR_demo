using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hub.Events
{
    public class NotificationEvent : INotification
    {
        public string Message { get; set; }
    }
}
