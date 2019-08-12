using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusRabbitMQ.Events
{
    public class NotificationEvent : INotification
    {
        public string Message { get; set; }
    }
}
