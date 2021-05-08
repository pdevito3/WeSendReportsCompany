namespace Reporting.IntegrationTests.FeatureTests.ReportRequest
{
    using Reporting.Core.Dtos.ReportRequest;
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using static Reporting.WebApi.Features.ReportRequests.GetReportRequestList;
    using static TestFixture;

    public class ReportRequestListQueryTests : TestBase
    {
        
        [Test]
        public async Task ReportRequestListQuery_Returns_Resource_With_Accurate_Props()
        {
            // Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            var fakeReportRequestTwo = new FakeReportRequest { }.Generate();
            var queryParameters = new ReportRequestParametersDto();

            await InsertAsync(fakeReportRequestOne, fakeReportRequestTwo);

            // Act
            var query = new ReportRequestListQuery(queryParameters);
            var reportRequests = await SendAsync(query);

            // Assert
            reportRequests.Should().HaveCount(2);
        }
        
        [Test]
        public async Task ReportRequestListQuery_Returns_Expected_Page_Size_And_Number()
        {
            //Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            var fakeReportRequestTwo = new FakeReportRequest { }.Generate();
            var fakeReportRequestThree = new FakeReportRequest { }.Generate();
            var queryParameters = new ReportRequestParametersDto() { PageSize = 1, PageNumber = 2 };

            await InsertAsync(fakeReportRequestOne, fakeReportRequestTwo, fakeReportRequestThree);

            //Act
            var query = new ReportRequestListQuery(queryParameters);
            var reportRequests = await SendAsync(query);

            // Assert
            reportRequests.Should().HaveCount(1);
        }
        
        [Test]
        public async Task ReportRequestListQuery_Returns_Sorted_ReportRequest_Provider_List_In_Asc_Order()
        {
            //Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            var fakeReportRequestTwo = new FakeReportRequest { }.Generate();
            fakeReportRequestOne.Provider = "bravo";
            fakeReportRequestTwo.Provider = "alpha";
            var queryParameters = new ReportRequestParametersDto() { SortOrder = "Provider" };

            await InsertAsync(fakeReportRequestOne, fakeReportRequestTwo);

            //Act
            var query = new ReportRequestListQuery(queryParameters);
            var reportRequests = await SendAsync(query);

            // Assert
            reportRequests
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeReportRequestTwo, options =>
                    options.ExcludingMissingMembers());
            reportRequests
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeReportRequestOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task ReportRequestListQuery_Returns_Sorted_ReportRequest_Provider_List_In_Desc_Order()
        {
            //Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            var fakeReportRequestTwo = new FakeReportRequest { }.Generate();
            fakeReportRequestOne.Provider = "bravo";
            fakeReportRequestTwo.Provider = "alpha";
            var queryParameters = new ReportRequestParametersDto() { SortOrder = "Provider" };

            await InsertAsync(fakeReportRequestOne, fakeReportRequestTwo);

            //Act
            var query = new ReportRequestListQuery(queryParameters);
            var reportRequests = await SendAsync(query);

            // Assert
            reportRequests
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeReportRequestTwo, options =>
                    options.ExcludingMissingMembers());
            reportRequests
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeReportRequestOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task ReportRequestListQuery_Returns_Sorted_ReportRequest_Target_List_In_Asc_Order()
        {
            //Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            var fakeReportRequestTwo = new FakeReportRequest { }.Generate();
            fakeReportRequestOne.Target = "bravo";
            fakeReportRequestTwo.Target = "alpha";
            var queryParameters = new ReportRequestParametersDto() { SortOrder = "Target" };

            await InsertAsync(fakeReportRequestOne, fakeReportRequestTwo);

            //Act
            var query = new ReportRequestListQuery(queryParameters);
            var reportRequests = await SendAsync(query);

            // Assert
            reportRequests
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeReportRequestTwo, options =>
                    options.ExcludingMissingMembers());
            reportRequests
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeReportRequestOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task ReportRequestListQuery_Returns_Sorted_ReportRequest_Target_List_In_Desc_Order()
        {
            //Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            var fakeReportRequestTwo = new FakeReportRequest { }.Generate();
            fakeReportRequestOne.Target = "bravo";
            fakeReportRequestTwo.Target = "alpha";
            var queryParameters = new ReportRequestParametersDto() { SortOrder = "Target" };

            await InsertAsync(fakeReportRequestOne, fakeReportRequestTwo);

            //Act
            var query = new ReportRequestListQuery(queryParameters);
            var reportRequests = await SendAsync(query);

            // Assert
            reportRequests
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeReportRequestTwo, options =>
                    options.ExcludingMissingMembers());
            reportRequests
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeReportRequestOne, options =>
                    options.ExcludingMissingMembers());
        }

        
        [Test]
        public async Task ReportRequestListQuery_Filters_ReportRequest_ReportRequestId()
        {
            //Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            var fakeReportRequestTwo = new FakeReportRequest { }.Generate();
            fakeReportRequestOne.ReportRequestId = Guid.NewGuid();
            fakeReportRequestTwo.ReportRequestId = Guid.NewGuid();
            var queryParameters = new ReportRequestParametersDto() { Filters = $"ReportRequestId == {fakeReportRequestTwo.ReportRequestId}" };

            await InsertAsync(fakeReportRequestOne, fakeReportRequestTwo);

            //Act
            var query = new ReportRequestListQuery(queryParameters);
            var reportRequests = await SendAsync(query);

            // Assert
            reportRequests.Should().HaveCount(1);
            reportRequests
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeReportRequestTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task ReportRequestListQuery_Filters_ReportRequest_Provider()
        {
            //Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            var fakeReportRequestTwo = new FakeReportRequest { }.Generate();
            fakeReportRequestOne.Provider = "alpha";
            fakeReportRequestTwo.Provider = "bravo";
            var queryParameters = new ReportRequestParametersDto() { Filters = $"Provider == {fakeReportRequestTwo.Provider}" };

            await InsertAsync(fakeReportRequestOne, fakeReportRequestTwo);

            //Act
            var query = new ReportRequestListQuery(queryParameters);
            var reportRequests = await SendAsync(query);

            // Assert
            reportRequests.Should().HaveCount(1);
            reportRequests
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeReportRequestTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task ReportRequestListQuery_Filters_ReportRequest_Target()
        {
            //Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            var fakeReportRequestTwo = new FakeReportRequest { }.Generate();
            fakeReportRequestOne.Target = "alpha";
            fakeReportRequestTwo.Target = "bravo";
            var queryParameters = new ReportRequestParametersDto() { Filters = $"Target == {fakeReportRequestTwo.Target}" };

            await InsertAsync(fakeReportRequestOne, fakeReportRequestTwo);

            //Act
            var query = new ReportRequestListQuery(queryParameters);
            var reportRequests = await SendAsync(query);

            // Assert
            reportRequests.Should().HaveCount(1);
            reportRequests
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeReportRequestTwo, options =>
                    options.ExcludingMissingMembers());
        }

    }
}