using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusRabbitMQ.Events
{
    //event is arise by rabbitmq consumer and handled in hub project to send notification message to the user -Sahil 12-08-2019
    public class NotificationEvent : INotification
    {
        public string Message { get; set; }
    }
}
