using EventBusRabbitMQ.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBusRabbitMQ
{
    //Add interface by bhavik yadav date:12/08/19
    public interface IRabbitMQOperation
    {

         //string RetriveMessage ();

        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>;
    }
}
