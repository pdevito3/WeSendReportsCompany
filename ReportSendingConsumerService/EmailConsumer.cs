namespace ReportSendingConsumerService
{
    using MassTransit;
    using Messages;
    using System.Threading.Tasks;

    public class EmailConsumer : IConsumer<ISendReportRequest>
    {
        public Task Consume(ConsumeContext<ISendReportRequest> context)
        {
            // do work to send email here

            return Task.CompletedTask;
        }
    }
}