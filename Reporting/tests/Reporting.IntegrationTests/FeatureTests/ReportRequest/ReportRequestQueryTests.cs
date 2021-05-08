namespace Reporting.IntegrationTests.FeatureTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using static Reporting.WebApi.Features.ReportRequests.GetReportRequest;
    using static TestFixture;

    public class ReportRequestQueryTests : TestBase
    {
        [Test]
        public async Task ReportRequestQuery_Returns_Resource_With_Accurate_Props()
        {
            // Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            await InsertAsync(fakeReportRequestOne);

            // Act
            var query = new ReportRequestQuery(fakeReportRequestOne.ReportRequestId);
            var reportRequests = await SendAsync(query);

            // Assert
            reportRequests.Should().BeEquivalentTo(fakeReportRequestOne, options =>
                options.ExcludingMissingMembers());
        }
    }
}