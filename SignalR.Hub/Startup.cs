using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Subscription;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SignalR.Hub.AutoFacModules;
using SignalR.Hub.Events;

namespace SignalR.Hub
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
            });

            //send message to user by bhavik yadav date:10/08/19
            //change cors policy for allow all connection from user -Sahil 19-08-2019
            services.AddCors(
                options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials());
                });

            //services.AddMediatR(typeof(NotificationEvent).Assembly);

            //add NLogger class DI -Sahil 13-08-2019
            //services.AddSingleton<INLogger, NLogger>();

            //inject rabbitmq class -Sahil 12-08-2019
            AddRabbitMQConfigs(services);

            services.AddSingleton<ISubscriptionsManager, InMemorySubscriptionsManager>();
            services.AddSingleton<INotificationHub, NotificationHub>();

            //configure autofac
            var container = new ContainerBuilder();
            container.RegisterModule(new ApplicationModule());
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        private void AddRabbitMQConfigs(IServiceCollection services)
        {
            //configure rabbitmq conneection
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBusConnection"]
                };

                if (!string.IsNullOrEmpty(Configuration["EventBusUserName"]))
                {
                    factory.UserName = Configuration["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(Configuration["EventBusPassword"]))
                {
                    factory.Password = Configuration["EventBusPassword"];
                }

                return new DefaultRabbitMQPersistentConnection(factory);
            });

            //Configure rabbitmq queue and channel  -Sahil 12-08-2019
            services.AddSingleton<IRabbitMQOperation>(sp =>
            {
                var connection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var queueName = Configuration["GlobalQueue"];
                //var iMediator = sp.GetRequiredService<IMediator>();
                //var nLogger = sp.GetRequiredService<INLogger>();
                var subManager = sp.GetRequiredService<ISubscriptionsManager>();
                var iLifeTimeScope = sp.GetRequiredService<ILifetimeScope>();

                return new RabbitMQOperation(connection, subManager, iLifeTimeScope, queueName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //send message to user by bhavik yadav date:10/08/19
            //use cors alllow all connection cors policy defined in ConfigureServices -Sahil 19-08-2019
            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationHub>("/notificationhub");
            });

            ConfigureEventBus(app);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IRabbitMQOperation>();
            eventBus.Subscribe<StockPriceChangedEvent, StockPriceChangedEventHandler>();
            eventBus.Subscribe<TickerDataChangeIntegrationEvent, TickerDataChangeIntegrationEventHandler>();
            eventBus.Subscribe<OrderbookDataChangeIntegrationEvent, OrderbookDataChangeIntegrationEventHandler>();
        }
    }
}
