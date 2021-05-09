namespace ReportSendingConsumerService
{
    using MassTransit;
    using Messages;
    using System.Threading.Tasks;

    public class CloudConsumer : IConsumer<ISendReportRequest>
    {
        public Task Consume(ConsumeContext<ISendReportRequest> context)
        {
            // do work to send cloud here

            return Task.CompletedTask;
        }
    }
}