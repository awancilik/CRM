using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ISAT.Business;

namespace ISAT.Web.Controllers
{
    [Authorize]
    public class InboxController : Controller
    {
        // GET: Inbox
        public ActionResult IndexInbox()
        {
            List<string> agenIdList = new List<string>();
            AgentInfo info = AgentInfo.GetAgentInfo(User.Identity.Name);
            agenIdList.Add(info.idCustAgent);
            if (info.IsSupervisor)
            {
                var infolist = AgentInfoList.GetAgentByGroup(info.GroupName).Where(x => x.SPVID == info.idCustAgent).ToList();
                foreach (var item in infolist)
                {
                    agenIdList.Add(item.idCustAgent);
                }
            }

            /// var obj = TicketInfoList.GetReportOpenTicket().Where(x => agenIdList.Contains(x.TicketOwner)).OrderByDescending(x => x.CreatedDate);
            var obj = TicketInfoList.GetReportOpenTicket().OrderByDescending(x => x.CreatedDate);
            ViewBag.TicketInfoList = obj;
            return View();
        }
    }
}