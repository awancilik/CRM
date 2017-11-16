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
    public class DashboardsController : Controller
    {
        // GET: Dashboards
        public ActionResult IndexDashboards()
        {
            DateTime baseDate = DateTime.Today;
            var today = baseDate;
            var yesterday = baseDate.AddDays(-1);
            var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
            var lastWeekStart = thisWeekStart.AddDays(-7);
            var lastWeekEnd = thisWeekStart.AddSeconds(-1);
            var thisMonthStart = baseDate.AddDays(1 - baseDate.Day);
            var thisMonthEnd = thisMonthStart.AddMonths(1).AddSeconds(-1);
            var lastMonthStart = thisMonthStart.AddMonths(-1);
            var lastMonthEnd = thisMonthStart.AddSeconds(-1);

            return View();
        }

        [HttpGet]
        public JsonResult DayToDay(string ticketType)
        {
            DateTime baseDate = DateTime.Today;
            DateTime startDate = baseDate.AddDays(-(int)baseDate.DayOfWeek);
            DateTime endDate = startDate.AddDays(7).AddSeconds(-1);
            TicketInfoList list = null;

            if (!string.IsNullOrEmpty(ticketType))
            {
                list = TicketInfoList.GetReportByDateAndTicketType(startDate, endDate, ticketType);
            }
            else
            {
                list = TicketInfoList.GetReportByDate(startDate, endDate);
            }

            List<TicketInfo> source = new List<TicketInfo>(); 
            List<string> agenIdList = new List<string>();
            AgentInfo info = AgentInfo.GetAgentInfo(User.Identity.Name);
            agenIdList.Add(info.idCustAgent);
            if (info.IsSupervisor)
            {
                var infolist = AgentInfoList.GetAgentByGroup(info.GroupName).Where(x=>x.SPVID==info.idCustAgent).ToList();
                foreach (var item in infolist)
                {
                    agenIdList.Add(item.idCustAgent);
                }
            }
             

            source = list.Where(x => agenIdList.Contains(x.CreatedBy)).ToList();
             
            List<MorrisBar> data = new List<MorrisBar>();

            for (DateTime i = startDate; i < endDate.AddDays(1); i=i.AddDays(1))
            {
                var tmpList = source.Where(x => x.CreatedDate.Date == i && agenIdList.Contains(x.TicketOwner)).ToList();
                MorrisBar bar = new MorrisBar();
                bar.Count = tmpList.Count;
                bar.Open = tmpList.Where(x => x.TicketStatus == "Open").ToList().Count;
                bar.Closed = tmpList.Where(x => x.TicketStatus == "Closed").ToList().Count;
                bar.Tanggal = i.Day;
                data.Add(bar);
            }


            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MonthToMonth(string ticketType)
        {
            DateTime baseDate = DateTime.Today;
            DateTime startDate = baseDate.AddDays(1 - baseDate.Day);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);
            TicketInfoList list = null;
            if (!string.IsNullOrEmpty(ticketType))
            {
                list = TicketInfoList.GetReportByDateAndTicketType(new DateTime(DateTime.Now.Year, 1, 1), new DateTime(DateTime.Now.Year, 12, 31), ticketType);
            }
            else
            {
                list = TicketInfoList.GetReportByDate(new DateTime(DateTime.Now.Year, 1, 1), new DateTime(DateTime.Now.Year, 12, 31));
            }

            List<TicketInfo> source = new List<TicketInfo>();
            List<string> agenIdList = new List<string>();
            AgentInfo info = AgentInfo.GetAgentInfo(User.Identity.Name);
            agenIdList.Add(User.Identity.Name);
            if (info.IsSupervisor)
            {
                var infolist = AgentInfoList.GetAgentByGroup(info.GroupName).Where(x => x.SPVID == info.idCustAgent).ToList();
                foreach (var item in infolist)
                {
                    agenIdList.Add(item.idCustAgent);
                }
            } 

            source = list.Where(x => agenIdList.Contains(x.CreatedBy)).ToList();

            List<MorrisBar> data = new List<MorrisBar>();

            for (int i = 1; i < 13; i++)
            {
                var tmpList = source.Where(x => x.CreatedDate.Month == i && agenIdList.Contains(x.TicketOwner)).ToList();
                MorrisBar m = new MorrisBar();
                m.Count = tmpList.Count;
                m.Closed = tmpList.Where(x => x.TicketStatus == "Closed").Count();
                m.Open = tmpList.Where(x => x.TicketStatus == "Open").Count();
                m.Tanggal = i;
                data.Add(m);
            } 
             
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AllCurrentMonthToMonth(string ticketType)
        {
            DateTime baseDate = DateTime.Today;
            DateTime startDate = baseDate.AddDays(1 - baseDate.Day);
            DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);
            List<TicketInfo> list = new List<TicketInfo>();

            if (!string.IsNullOrEmpty(ticketType))
            {
                list = TicketInfoList.GetReportByDateAndTicketType(startDate, endDate, ticketType).ToList();
            }
            else
            {
                list = TicketInfoList.GetReportByDate(startDate, endDate).ToList();
            }

            List<TicketInfo> source = new List<TicketInfo>();
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

            source = list.Where(x => agenIdList.Contains(x.TicketOwner)).ToList();

            List<MorrisDonut> data = new List<MorrisDonut>();

            MorrisDonut m = new MorrisDonut();
            m.label = "Open";
            m.value = source.Where(x => x.TicketStatus == "Open").Count();
            data.Add(m);

            m = new MorrisDonut();
            m.value = source.Where(x => x.TicketStatus == "Closed").Count();
            m.label = "Closed";
            data.Add(m);


            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGata()
        {
            try
            {
                List<TicketUpdate> list = new List<TicketUpdate>();

                AgentInfo info = AgentInfo.GetAgentInfo(User.Identity.Name);
                List<string> agenIdList = new List<string>();
                agenIdList.Add(info.idCustAgent);
                if (info.IsSupervisor)
                {
                    var infolist = AgentInfoList.GetAgentByGroup(info.GroupName).Where(x => x.SPVID == info.idCustAgent).ToList();
                    foreach (var item in infolist)
                    {
                        agenIdList.Add(item.idCustAgent);
                    }
                }

                var data = TicketInfoList.GetTicketInfoList().Where(x=> agenIdList.Contains(x.TicketOwner)).ToList();

                var val = from c in data
                          select new TicketUpdate
                          {
                              TicketID = c.TicketNo,
                              Status = c.TicketStatus,
                              Owner = c.TicketOwner,
                              UpdatedDate = c.UpdatedDate.ToString("dd MMM yyyy")
                          };

                return Json(val, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public JsonResult GetGata2(string ticketType)
        {
            try
            {
                List<TicketUpdate> list = new List<TicketUpdate>();

                AgentInfo info = AgentInfo.GetAgentInfo(User.Identity.Name);
                List<string> agenIdList = new List<string>();
                agenIdList.Add(info.idCustAgent);
                if (info.IsSupervisor)
                {
                    var infolist = AgentInfoList.GetAgentByGroup(info.GroupName).Where(x => x.SPVID == info.idCustAgent).ToList();
                    foreach (var item in infolist)
                    {
                        agenIdList.Add(item.idCustAgent);
                    }
                }

                var data = TicketInfoList.GetTicketInfoList().Where(x => agenIdList.Contains(x.TicketOwner) && x.TicketType == ticketType).ToList();

                var val = from c in data
                          select new TicketUpdate
                          {
                              TicketID = c.TicketNo,
                              Status = c.TicketStatus,
                              Owner = c.TicketOwner,
                              UpdatedDate = c.UpdatedDate.ToString("dd MMM yyyy")
                          };

                return Json(val.ToArray(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        //public JsonResult GetAjaxData(JQueryDataTableParamModel param)
        //{
        //    TicketInfoList list = TicketInfoList.GetTicketInfoList();
        //    var totalRowsCount = list.Count;
        //    var filteredRowsCount = new System.Data.Objects.ObjectParameter("FilteredRowsCount", typeof(int));
        //    var data = list e.pr_SearchPerson(param.sSearch,
        //                    Convert.ToInt32(Request["iSortCol_0"]),
        //                    Request["sSortDir_0"],
        //                    param.iDisplayStart,
        //                    param.iDisplayStart + param.iDisplayLength,
        //                    totalRowsCount,
        //                    filteredRowsCount);
        //    var aaData = data.Select(d => new string[] { d.FirstName, d.LastName, d.Nationality, d.DateOfBirth.Value.ToString("dd MMM yyyy") }).ToArray();
        //    return Json(new
        //    {
        //        sEcho = param.sEcho,
        //        aaData = aaData,
        //        iTotalRecords = Convert.ToInt32(totalRowsCount.Value),
        //        iTotalDisplayRecords = Convert.ToInt32(filteredRowsCount.Value)
        //    }, JsonRequestBehavior.AllowGet);

        //}

    }
}