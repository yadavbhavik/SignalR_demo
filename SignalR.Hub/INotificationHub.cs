using SignalR.Hub.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hub
{
    interface INotificationHub
    {
        /*espay-layout-beta send constant data for specific time duration -Sahil 22-08-2019
            Params: tickerDataChangeIntegrationEvent = data to be send e.x price change
             
            Use: send data to the signalR client connection group wise
         */
        Task<int> TickerData(TickerDataChangeIntegrationEvent tickerDataChangeIntegrationEvent);


        /*espay-layout-beta send sell and buy orderbook data to the signalR connection group wise -Sahil 22-08-2019
           Params: Type =  name of signalR group prefix e.x BuyerBook or SellerBook
                   Pair =  Currency pair  define group name concatenate with Type
                   Data = data to be passed

           Use: send data to the signalR client connection group wise
            */
        Task<int> SellerBookLP(string Type, string Pair, string Data);
        Task<int> BuyerBookLP(string Type, string Pair, string Data);
    }
}
