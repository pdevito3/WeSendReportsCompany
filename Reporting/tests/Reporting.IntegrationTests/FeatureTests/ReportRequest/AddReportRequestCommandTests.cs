namespace Reporting.IntegrationTests.FeatureTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using static Reporting.WebApi.Features.ReportRequests.AddReportRequest;
    using static TestFixture;

    public class AddReportRequestCommandTests : TestBase
    {
        [Test]
        public async Task AddReportRequestCommand_Adds_New_ReportRequest_To_Db()
        {
            // Arrange
            var fakeReportRequestOne = new FakeReportRequestForCreationDto { }.Generate();

            // Act
            var command = new AddReportRequestCommand(fakeReportRequestOne);
            var reportRequestReturned = await SendAsync(command);
            var reportRequestCreated = await ExecuteDbContextAsync(db => db.ReportRequests.SingleOrDefaultAsync());

            // Assert
            reportRequestReturned.Should().BeEquivalentTo(fakeReportRequestOne, options =>
                options.ExcludingMissingMembers());
            reportRequestCreated.Should().BeEquivalentTo(fakeReportRequestOne, options =>
                options.ExcludingMissingMembers());
        }
    }
}