using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISAT.Web.ViewModel;
using ISAT.Business;

namespace ISAT.Web.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult IndexReport()
        {
            List<ISAT.Web.ViewModel.ReportVM> list = new List<ViewModel.ReportVM>();
            string username = User.Identity.Name;

            List<string> agenIdList = new List<string>();
            AgentInfo info = AgentInfo.GetAgentInfo(username);
            agenIdList.Add(info.idCustAgent);
            if (info.IsSupervisor)
            {
                var infolist = AgentInfoList.GetAgentByGroup(info.GroupName).Where(x => x.SPVID == info.idCustAgent).ToList();
                foreach (var item in infolist)
                {
                    agenIdList.Add(item.idCustAgent);
                }
            }

            var openList = ISAT.Business.TicketInfoList.GetReportOpenTicket().Where(x => agenIdList.Contains(x.TicketOwner)).OrderByDescending(x => x.CreatedDate);
            var closedList = ISAT.Business.TicketInfoList.GetReportCloseTicket().Where(x => agenIdList.Contains(x.TicketOwner)).OrderByDescending(x => x.CreatedDate);

            var newopenList = from c in openList
                              select new ReportVM
                              {
                                  TicketId = c.idticket,
                                  Subject = c.TicketSubject,
                                  NamaCustomer = c.NamaCustomer,
                                  Requester = GetRequesterFullName(c.Requester),
                                  CreatedDate = c.CreatedDate.ToString("dd MMM yyyy HH':'mm"),
                                  UpdatedDate = c.UpdatedDate.ToString("dd MMM yyyy HH':'mm"),
                                  Priority = c.Priority,
                                  TicketOwner = c.TicketOwner,
                                  Status = c.TicketStatus
                              };

            var newcloseList = from c in closedList
                               select new ReportVM
                               {
                                   TicketId = c.idticket,
                                   Subject = c.TicketSubject,
                                   NamaCustomer = c.NamaCustomer,
                                   Requester = GetRequesterFullName(c.Requester),
                                   CreatedDate = c.CreatedDate.ToString("dd MMM yyyy HH':'mm"),
                                   UpdatedDate = c.UpdatedDate.ToString("dd MMM yyyy HH':'mm"),
                                   Priority = c.Priority,
                                   TicketOwner = c.TicketOwner,
                                   Status = c.TicketStatus
                               };

            list.AddRange(newopenList);
            list.AddRange(newcloseList);
            return View(list);
        }

        private string GetRequesterFullName(string emailid)
        {
            CustomerInfo info = CustomerInfo.GetCustomerInfo(emailid);
            if (info != null)
            {
                return info.FirstName + " " + info.LastName;
            }
            return string.Empty;
        }

        public ActionResult Open()
        {
            return View();
        }

        public ActionResult Close()
        {
            return View();
        }
    }
}