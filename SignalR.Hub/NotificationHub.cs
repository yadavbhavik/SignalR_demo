using System;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hub
{
    public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        //add configuration by bhavik yadav date:09/08/19
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            await base.OnDisconnectedAsync(ex);
        }

       //send message to user by bhavik yadav date:09/08/19
        public async Task SendWelcomeMessage()
        {
            await Clients.All.SendAsync("WelcomeMessage","Welcome user...");
        }
    }

}
