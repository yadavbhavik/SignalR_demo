using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hub.Events
{
    public class NotificationEvent : INotification
    {
        //event is arise by rabbitmq consumer and handled in hub project to send notification message to the user -Sahil 12-08-2019
        public string Message { get; set; }
    }
}
