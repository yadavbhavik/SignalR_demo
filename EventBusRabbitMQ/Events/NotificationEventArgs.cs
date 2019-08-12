using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusRabbitMQ.Events
{
    public class NotificationEventArgs : System.EventArgs
    {
        public NotificationEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
