using GloboTicket.Services.Ordering.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GloboTicket.Services.Ordering.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IAzServiceBusConsumer ServiceBusConsumer { get; set; }

        public static IApplicationBuilder UseAzServiceBusConsumer(this IApplicationBuilder app)
        {
            ServiceBusConsumer = app.ApplicationServices.GetService<IAzServiceBusConsumer>();
            var hostApplicationLifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
            hostApplicationLifetime.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            ServiceBusConsumer.Start();
        }

        private static void OnStopping()
        {
            ServiceBusConsumer.Stop();
        }
    }
}
