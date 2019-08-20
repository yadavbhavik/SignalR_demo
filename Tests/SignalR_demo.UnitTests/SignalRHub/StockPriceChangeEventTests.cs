using Microsoft.AspNetCore.SignalR;
using Moq;
using SignalR.Hub;
using SignalR.Hub.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace SignalR_demo.UnitTests.SignalRHub
{
    public class StockPriceChangeEventTests
    {
        private readonly Mock<IHubContext<NotificationHub>> mockHubContext;
        public StockPriceChangeEventTests()
        {
            mockHubContext = new Mock<IHubContext<NotificationHub>>();
        }

        [Fact]
        public void Handle_Send_Messsage_To_Client()
        {
            //Arrange
            StockPriceChangedEvent mockEvent = new StockPriceChangedEvent(message: "Mock Message");
            StockPriceChangedEventHandler handler = new StockPriceChangedEventHandler(mockHubContext.Object);

            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            mockHubContext.Setup(h => h.Clients.All).Returns(mockClientProxy.Object);

            //Act
            handler.Handle(mockEvent);

            //Assert
            mockClients.Verify(clients => clients.All, Times.Never);
            mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                        "UpdatedOrderState",
                        It.Is<object[]>(o => o != null),
                        default(CancellationToken)),
                        Times.Once
                );

        }
    }
}
