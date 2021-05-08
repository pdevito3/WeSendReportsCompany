namespace ReportSendingConsumerService
{
    using MassTransit;
    using Messages;
    using System.Threading.Tasks;

    public class FaxConsumer : IConsumer<ISendReportRequest>
    {
        public Task Consume(ConsumeContext<ISendReportRequest> context)
        {
            // do work to send fax here

            return Task.CompletedTask;
        }
    }
}