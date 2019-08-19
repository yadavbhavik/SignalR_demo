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

        [Fact(Skip ="DoSubscription method testing has not done yet")]
        public void AddSubscription_Not_Add_EventTypes_If_Exists()
        {
            //Arrange

            //add event already into eventType list
            inMemorySubscriptionsManager.AddSubscription<StockPriceChangedEvent, StockPriceChangedEventHandler>();
            var preEventTypeCount = inMemorySubscriptionsManager.EventTypeCountTest();

            //Act

            //again add same event for check if contains validation is working or not
            inMemorySubscriptionsManager.AddSubscription<StockPriceChangedEvent, StockPriceChangedEventHandler>();
            var eventTypeCount = inMemorySubscriptionsManager.EventTypeCountTest();

            //Assert
            Assert.Equal(preEventTypeCount, eventTypeCount);          
        }

        [Fact]
        public void DoAddSubscription_Throws_ArgumentException_Check()
        {
            //Arrange
            var handlerType = typeof(StockPriceChangedEventHandler);
            var eventName = "StockPriceChange";
            inMemorySubscriptionsManager.DoAddSubscriptionTest(handlerType, eventName);

            //Act-Assert
            Assert.Throws<ArgumentException>(() =>
            inMemorySubscriptionsManager.DoAddSubscriptionTest(handlerName: handlerType, eventName: eventName));
        }
    }
}
