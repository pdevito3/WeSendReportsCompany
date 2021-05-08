namespace Reporting.FunctionalTests.FunctionalTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class DeleteReportRequestTests : TestBase
    {
        [Test]
        public async Task Delete_ReportRequestReturns_NoContent()
        {
            // Arrange
            var fakeReportRequest = new FakeReportRequest { }.Generate();

            await InsertAsync(fakeReportRequest);

            // Act
            var route = ApiRoutes.ReportRequests.Delete.Replace(ApiRoutes.ReportRequests.ReportRequestId, fakeReportRequest.ReportRequestId.ToString());
            var result = await _client.DeleteRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}