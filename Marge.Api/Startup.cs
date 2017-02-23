﻿using System;
using Marge.Common.Events;
using Marge.Core.Commands;
using Marge.Core.Commands.Handlers;
using Marge.Core.Queries;
using Marge.Core.Queries.Handlers;
using Marge.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Marge.Api
{
    public class Startup
    {
        private static IDisposable[] subscriptions;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddSingleton<IEventStore, InMemoryEventStore>();
            services.AddSingleton<CommandBus>();
            services.AddSingleton<EventBus>();
            services.AddSingleton<PriceCommandHandler>();
            services.AddSingleton<UpdatePricesHandler>();

            var priceRepository = new PriceRepository();
            services.AddSingleton<PriceRepository>(priceRepository);
            services.AddSingleton<IPriceQuery>(priceRepository);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            ConfigureCommandBus(app);
            ConfigureQueryHandlers(app);
        }

        private static void ConfigureCommandBus(IApplicationBuilder app)
        {
            var commandBus = app.ApplicationServices.GetService<CommandBus>();
            var commandHandler = app.ApplicationServices.GetService<PriceCommandHandler>();
            commandBus.Subscribe<ChangeDiscountCommand>(commandHandler);
            commandBus.Subscribe<CreatePriceCommand>(commandHandler);
        }

        private static void ConfigureQueryHandlers(IApplicationBuilder app)
        {
            var updatePricesHandler = app.ApplicationServices.GetService<UpdatePricesHandler>();
            var eventBus = app.ApplicationServices.GetService<EventBus>();
            subscriptions = new[]
            {
                eventBus.Subscribe<DiscountChanged>(updatePricesHandler.Handle),
                eventBus.Subscribe<PriceCreated>(updatePricesHandler.Handle),
            };
        }
    }
}
