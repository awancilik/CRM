using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISAT.Web.ViewModel
{
    public class Report
    {
        public ISAT.Business.TicketInfoList Open { get; set; }
        public ISAT.Business.TicketInfoList Close { get; set; }
        public ISAT.Business.TicketInfoList Escalated { get; set; }

        public Report(string username)
        {
            this.Open = ISAT.Business.TicketInfoList.GetReportOpenTicketCurrentUser(username);
            this.Close = ISAT.Business.TicketInfoList.GetReportCloseTicketCurrentUser(username);
            this.Escalated = ISAT.Business.TicketInfoList.GetReportCloseTicket();

        }
    }


    public class ReportVM
    {
        public long TicketId { get; set; }
        public string Subject { get; set; }
        public string NamaCustomer { get; set; }
        public string Requester { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string Priority { get; set; }
        public string TicketOwner { get; set; }
        public string Status { get; set; }

    }
}