using System;
using Marge.Common.Events;
using Marge.Core.Commands.Handlers;
using Marge.Core.Queries;
using Marge.Core.Queries.Data;
using Marge.Core.Queries.Handlers;
using Marge.Infrastructure;
using Marge.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Marge.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
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

            ConfigureDependencies(services);
        }

        private static void ConfigureDependencies(IServiceCollection services)
        {
            var priceRepository = new PriceRepository();

            services.AddSingleton<IEventStore, EventStore>()
                .AddSingleton<IEventBus, EventBus>()
                .AddSingleton<ICommandBus, CommandBus>()
                .AddSingleton<IEventAggregateCommandHandler, EventAggregateCommandHandler>()
                .AddSingleton<UpdatePricesHandler>()
                .AddSingleton<IPriceSaver>(priceRepository)
                .AddSingleton<IPriceQuery>(priceRepository)
                .AddSingleton<ITransactionFactory, TransactionFactory>()
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
            app.UseMvc();

            ConfigureCommandBus(app);
            ConfigureQueryHandlers(app);
        }

        private static void ConfigureCommandBus(IApplicationBuilder app)
        {
            PriceCommandHandler.RegisterCommands(app.ApplicationServices.GetService<ICommandBus>());
        }

        private static void ConfigureQueryHandlers(IApplicationBuilder app)
        {
            var updatePricesHandler = app.ApplicationServices.GetService<UpdatePricesHandler>();
            var eventBus = app.ApplicationServices.GetService<IEventBus>();
            eventBus.Subscribe<DiscountChanged>(updatePricesHandler.Handle);
            eventBus.Subscribe<PriceCreated>(updatePricesHandler.Handle);
        }
    }
}
