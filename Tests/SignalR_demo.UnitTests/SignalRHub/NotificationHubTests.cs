using Microsoft.AspNetCore.SignalR;
using Moq;
using SignalR.Hub;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SignalR_demo.UnitTests.SignalRHub
{
    public class NotificationHubTests
    {
        [Fact]
        public async Task SignalR_UserWelcome_MessageReturn_TestAsync()
        {
            //Arrange
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            NotificationHub hub = new NotificationHub()
            {
                Clients = mockClients.Object
            };

            //Act 
            await hub.SendWelcomeMessage();

            //Assert
            mockClients.Verify(clients => clients.All, Times.Once);
            mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(
                        "WelcomeMessage",
                        It.Is<object[]>(o => o != null),
                        default(CancellationToken)),
                        Times.Once
                );

        }
    }
}
