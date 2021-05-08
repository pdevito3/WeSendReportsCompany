namespace Reporting.IntegrationTests.FeatureTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.IntegrationTests.TestUtilities;
    using Reporting.Core.Dtos.ReportRequest;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.JsonPatch;
    using System.Linq;
    using static Reporting.WebApi.Features.ReportRequests.PatchReportRequest;
    using static TestFixture;

    public class PatchReportRequestCommandTests : TestBase
    {
        [Test]
        public async Task PatchReportRequestCommand_Updates_Existing_ReportRequest_In_Db()
        {
            // Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            await InsertAsync(fakeReportRequestOne);
            var reportRequest = await ExecuteDbContextAsync(db => db.ReportRequests.SingleOrDefaultAsync());
            var reportRequestId = reportRequest.ReportRequestId;

            var patchDoc = new JsonPatchDocument<ReportRequestForUpdateDto>();
            var newValue = "Easily Identified Value For Test";
            patchDoc.Replace(r => r.Provider, newValue);

            // Act
            var command = new PatchReportRequestCommand(reportRequestId, patchDoc);
            await SendAsync(command);
            var updatedReportRequest = await ExecuteDbContextAsync(db => db.ReportRequests.Where(r => r.ReportRequestId == reportRequestId).SingleOrDefaultAsync());

            // Assert
            updatedReportRequest.Provider.Should().Be(newValue);
        }
    }
}