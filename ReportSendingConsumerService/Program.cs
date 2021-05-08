namespace ReportSendingConsumerService
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MassTransit;
    using RabbitMQ.Client;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(mt =>
                    {
                        mt.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("localhost", "/", h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });

                            cfg.ReceiveEndpoint("email-reports", re =>
                            {
                                re.ConfigureConsumeTopology = false;
                                re.Consumer<EmailConsumer>();
                                re.Bind("report-requests", e =>
                                {
                                    e.RoutingKey = "email";
                                    e.ExchangeType = ExchangeType.Direct;
                                });
                            });
                            cfg.ReceiveEndpoint("fax-reports", re =>
                            {
                                re.ConfigureConsumeTopology = false;
                                re.Consumer<FaxConsumer>();
                                re.Bind("report-requests", e =>
                                {
                                    e.RoutingKey = "fax";
                                    e.ExchangeType = ExchangeType.Direct;
                                });
                            });
                        });
                    });
                    services.AddMassTransitHostedService();

                    services.AddHostedService<Worker>();
                });
    }
}