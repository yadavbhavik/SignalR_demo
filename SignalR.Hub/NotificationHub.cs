using System;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Logging;
using Microsoft.Extensions.Configuration;

namespace SignalR.Hub
{
    public class NotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IConfiguration configuration;

        public NotificationHub(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        //add configuration by bhavik yadav date:09/08/19
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "test_notification");

            //espay-layout-beta project signalR client connection group -Sahil 21-08-2019

            //get default pair. when client connection created all client group into Default pair defined in appsettings -Sahil 21-08-2019
            string Pair = configuration.GetValue<string>("SignalRKey:DefaultPair");

            Groups.AddToGroupAsync(Context.ConnectionId, "BuyerBook:" + Pair).Wait();
            Groups.AddToGroupAsync(Context.ConnectionId, "SellerBook:" + Pair).Wait();


            //espay-layout-beta Arbitrage Connection -Sahil 21-08-2019
            string BaseCurrencyArbitrage = configuration.GetValue<string>("SignalRKey:DefaultBaseCurrencyArbitrage");
            Groups.AddToGroupAsync(Context.ConnectionId, "PairDataArbitrage:" + BaseCurrencyArbitrage).Wait();

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

        }
    }

}
