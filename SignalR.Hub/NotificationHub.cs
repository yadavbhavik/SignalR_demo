using System;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Logging;

namespace SignalR.Hub
{
    public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IRabbitMQOperation mQOperation;

        public NotificationHub(IRabbitMQOperation mQOperation)
        {
            this.mQOperation = mQOperation;
        }
        //add configuration by bhavik yadav date:09/08/19
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "test_notification");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "test_notification");
            await base.OnDisconnectedAsync(ex);
        }

        //send message to user by bhavik yadav date:09/08/19
        public async Task SendWelcomeMessage()
        {
            NLogger.WriteLogIntoFile("SendWelcomeMessage", "", "welcome message called.");
            await Clients.All.SendAsync("WelcomeMessage", "Testing...testing...welcome user.");
            NLogger.WriteLogIntoFile("SendWelcomeMessage", "", "welcome message called, RetiveMessage:"+ mQOperation.RetriveMessage());

        }
    }

}
