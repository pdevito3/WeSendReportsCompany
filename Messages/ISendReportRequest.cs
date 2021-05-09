namespace Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISendReportRequest
    {
        Guid ReportId { get; set; }
        string Provider { get; set; } // "email" or "fax" or "cloud"
        string Target { get; set; }
        public bool IsPublic { get; set; }
    }
}