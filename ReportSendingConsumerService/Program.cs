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

                            cfg.ReceiveEndpoint("private-reports", re =>
                            {
                                // turns off default fanout settings
                                re.ConfigureConsumeTopology = false;

                                // a replicated queue to provide high availability and data safety. available in RMQ 3.8+
                                re.SetQuorumQueue();

                                // enables a lazy queue for more stable cluster with better predictive performance.
                                // Please note that you should disable lazy queues if you require really high performance, if the queues are always short, or if you have set a max-length policy.
                                re.SetQueueArgument("declare", "lazy");

                                re.Consumer<EmailConsumer>();
                                re.Consumer<FaxConsumer>();
                                re.Bind("send-report", e =>
                                {
                                    e.RoutingKey = "private.*";
                                    e.ExchangeType = ExchangeType.Topic;
                                });
                            });
                            cfg.ReceiveEndpoint("public-reports", re =>
                            {
                                // turns off default fanout settings
                                re.ConfigureConsumeTopology = false;

                                // a replicated queue to provide high availability and data safety. available in RMQ 3.8+
                                re.SetQuorumQueue();

                                // enables a lazy queue for more stable cluster with better predictive performance.
                                // Please note that you should disable lazy queues if you require really high performance, if the queues are always short, or if you have set a max-length policy.
                                re.SetQueueArgument("declare", "lazy");

                                re.Consumer<CloudConsumer>();
                                re.Bind("send-report", e =>
                                {
                                    e.RoutingKey = "public.*";
                                    e.ExchangeType = ExchangeType.Topic;
                                });
                            });
                        });
                    });
                    services.AddMassTransitHostedService();

                    services.AddHostedService<Worker>();
                });
    }
}