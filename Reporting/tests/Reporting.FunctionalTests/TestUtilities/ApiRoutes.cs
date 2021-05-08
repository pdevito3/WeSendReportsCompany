namespace Reporting.FunctionalTests.TestUtilities
{
    public class ApiRoutes
    {
        public const string Base = "api";
        public const string Health = Base + "/health";        

public static class ReportRequests
        {
            public const string ReportRequestId = "{reportRequestId}";
            public const string GetList = Base + "/reportRequests";
            public const string GetRecord = Base + "/reportRequests/" + ReportRequestId;
            public const string Create = Base + "/reportRequests";
            public const string Delete = Base + "/reportRequests/" + ReportRequestId;
            public const string Put = Base + "/reportRequests/" + ReportRequestId;
            public const string Patch = Base + "/reportRequests/" + ReportRequestId;
        }

        // new api route marker - do not delete
    }
}