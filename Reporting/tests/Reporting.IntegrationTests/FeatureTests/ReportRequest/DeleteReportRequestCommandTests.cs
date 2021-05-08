namespace Reporting.IntegrationTests.FeatureTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using static Reporting.WebApi.Features.ReportRequests.DeleteReportRequest;
    using static TestFixture;

    public class DeleteReportRequestCommandTests : TestBase
    {
        [Test]
        public async Task DeleteReportRequestCommand_Deletes_ReportRequest_From_Db()
        {
            // Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            await InsertAsync(fakeReportRequestOne);
            var reportRequest = await ExecuteDbContextAsync(db => db.ReportRequests.SingleOrDefaultAsync());
            var reportRequestId = reportRequest.ReportRequestId;

            // Act
            var command = new DeleteReportRequestCommand(reportRequestId);
            await SendAsync(command);
            var reportRequests = await ExecuteDbContextAsync(db => db.ReportRequests.ToListAsync());

            // Assert
            reportRequests.Count.Should().Be(0);
        }
    }
}