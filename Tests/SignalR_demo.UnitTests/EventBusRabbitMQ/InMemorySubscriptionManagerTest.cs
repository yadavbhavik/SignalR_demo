using EventBusRabbitMQ.Subscription;
using SignalR.Hub.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SignalR_demo.UnitTests.EventBusRabbitMQ
{
   public class InMemorySubscriptionManagerTest
    {
        private readonly InMemorySubscriptionsManager inMemorySubscriptionsManager;
        public InMemorySubscriptionManagerTest() {
            inMemorySubscriptionsManager = new InMemorySubscriptionsManager();
        }

        [Fact]
        public void GetEventKey_Valid_Return_Check()
        {
            //Arrange 
            var typeName = typeof(StockPriceChangedEvent).Name;

            //Act
            var returnKey = inMemorySubscriptionsManager.GetEventKey<StockPriceChangedEvent>();

            //Assert
            Assert.Same(typeName, returnKey);
        }

        [Fact]
        public void GetEventKey_NotValidAndNull_Return_Check()
        {
            //Arrange-Act
            var returnKey = inMemorySubscriptionsManager.GetEventKey<StockPriceChangedEvent>();

            //Assert
            Assert.NotNull(returnKey);
        }
    }
}
