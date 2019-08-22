using EventBusRabbitMQ.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Hub.Events
{
    public class TickerDataChangeIntegrationEvent : IntegrationEvent
    {
        public decimal LTP { get; }
        public string Pair { get; }
        public short LPType { get; }
        public string LPName { get; }
        public decimal Volume { get; }
        public decimal Fees { get; }
        public decimal ChangePer { get; }
        public short UpDownBit { get; }

        public TickerDataChangeIntegrationEvent(decimal lTP, string pair, short lPType, string lPName, decimal volume, decimal fees, decimal changePer, short upDownBit)
        {
            LTP = lTP;
            Pair = pair;
            LPType = lPType;
            LPName = lPName;
            Volume = volume;
            Fees = fees;
            ChangePer = changePer;
            UpDownBit = upDownBit;
        }
    }
}
