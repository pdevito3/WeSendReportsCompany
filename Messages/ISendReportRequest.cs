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
        string Provider { get; set; } // "email" or "fax"
        string Target { get; set; }
    }
}