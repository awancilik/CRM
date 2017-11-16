using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISAT.Business;
using ISAT.Web.Models;
using Newtonsoft.Json;

namespace ISAT.Web.Controllers
{
    [Authorize]
    public class TicketingController : Controller
    {
        TicketVM ticketmodel = new TicketVM();
        // GET: Ticketing
        public ActionResult Index(string email, string phoneNumber)
        {
            Ticketing ticket = Ticketing.Create(User.Identity.Name, email);
            ViewModel.Ticket m = new ViewModel.Ticket(ticket.TicketObject, ticket.CustObject);
            if (string.IsNullOrEmpty(m.Requester.Custno))
            {
                m.Requester.Custno = phoneNumber;
            }

            return View(m);
        }

        public ActionResult CallerAction(string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                CustomerInfo info = CustomerInfo.GetCustomerInfoByPhone(phoneNumber.Trim());
                string email = "00000000000000";
                if (info != null)
                {
                    email = info.Email;
                }
                return RedirectToAction("Index", new { email = email, phoneNumber = phoneNumber });
            }
            return RedirectToAction("Index");
        }

        public ActionResult Inbox()
        {
            ISAT.Business.TicketInfoList list = ISAT.Business.TicketInfoList.GetTicketInbox(User.Identity.Name);
            return View(list);
        }

        public ActionResult AddSolution(string id)
        {
            Ticket ticket = Ticket.GetTicket(id);
            Customer cust = Customer.GetCustomer(ticket.Requester);
            if (cust == null)
            {
                cust = Customer.NewCustomer();
                return RedirectToAction("Index", new { email = cust.Email, phoneNumber = cust.CustNo });
            }
            ViewModel.Ticket vm = new ViewModel.Ticket(ticket, cust);
            return View(vm);
        }

        [HttpGet]
        public JsonResult AddItemSolution(string solution)
        {
            ISAT.Web.ViewModel.TicketSolution m = new ViewModel.TicketSolution();
            m.Solution = solution;
            m.UpdateBy = User.Identity.Name;
            m.UpdateDate = DateTime.Now;
            return Json(m, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddSolution(ViewModel.Ticket m)
        {
            try
            {
                foreach (var item in m.Solution)
                {
                    if (item.UpdateDate==DateTime.MinValue)
                    {
                        item.UpdateDate = DateTime.Now;
                    }
                }

                Ticket old = Ticket.GetTicket(m.IdTicket);
                Ticket obj = ViewModel.Ticket.ConvertToTicketBusiness(m);
                
                obj.Requester = m.Requester.Email;
                obj.TicketStatus = TicketStatus.Open.ToString();
                obj.Escalation = User.Identity.Name;
                //obj.CreatedBy = User.Identity.Name;
                //obj.CreatedDate = DateTime.Now;
                obj.UpdatedBy = User.Identity.Name;
                obj.UpdatedDate = DateTime.Now;
                //string updatecommand = GetUpdateCommand(m, old);

                

                var objHistory = SaveTicketHistory(obj, "Add Solution");

                if (obj.IsValid && objHistory.IsValid)
                {
                    obj = obj.Save();
                    objHistory = objHistory.Save();
                }

                ViewBag.SuccessMsg = "Success";
            }
            catch (Exception ex)
            {
                return View(m);
            }

            return RedirectToAction("IndexInbox", "Inbox");
        }

        public ActionResult UpdateTicket(string id)
        {
            Ticket ticket = Ticket.GetTicket(id);
            Customer cust = Customer.GetCustomer(ticket.Requester);
            if (cust == null)
            {
                cust = Customer.NewCustomer();
            }
            ViewModel.Ticket vm = new ViewModel.Ticket(ticket, cust);
            return View(vm);
        }

        [HttpPost]
        public ActionResult UpdateTicket(ViewModel.Ticket m)
        {
            try
            {

                foreach (var item in m.Solution)
                {
                    if (item.UpdateDate == DateTime.MinValue)
                    {
                        item.UpdateDate = DateTime.Now;
                    }
                }

                Ticket old = Ticket.GetTicket(m.IdTicket);
                Ticket obj = ViewModel.Ticket.ConvertToTicketBusiness(m);
                obj.Requester = m.Requester.Email;
                obj.TicketStatus = TicketStatus.Open.ToString();
                obj.Escalation = User.Identity.Name;
                //obj.CreatedBy = User.Identity.Name;
                //obj.CreatedDate = DateTime.Now;
                obj.UpdatedBy = User.Identity.Name;
                obj.UpdatedDate = DateTime.Now;
                //string updatecommand = GetUpdateCommand(m, old);
                var objHistory = SaveTicketHistory(obj, "Update Ticket");

                Customer cust = Customer.GetCustomer(obj.Requester);
                if (cust!=null)
                {
                    cust.FirstName = m.Requester.FirstName;
                    cust.LastName = m.Requester.LastName;
                    cust.CustNo = m.Requester.Custno;
                    cust.CompanyName = m.Requester.CompanyName;

                    if (cust.IsValid)
                    {
                        cust = cust.Save();
                    }
                }

                if (obj.IsValid && objHistory.IsValid)
                {
                    obj = obj.Save();
                    objHistory = objHistory.Save();
                }

                ViewBag.SuccessMsg = "Success";
            }
            catch (Exception ex)
            {
                return View(m);
            }

            return RedirectToAction("IndexInbox", "Inbox");
        }

        [HttpPost]
        public ActionResult Index(ViewModel.Ticket m)
        {
            string errorMessage = string.Empty;
            try
            {

                foreach (var item in m.Solution)
                {
                    if (item.UpdateDate == DateTime.MinValue)
                    {
                        item.UpdateDate = DateTime.Now;
                    }
                }

                using (DatabaseManager ctx = new DatabaseManager(true))
                {
                    if (string.IsNullOrEmpty(m.TicketNo))
                    {
                        m.TicketNo = TicketNo.Create(User.Identity.Name, "TICKET");
                    }

                    Ticket obj = ViewModel.Ticket.ConvertToTicketBusiness(m);
                    obj.Requester = m.Requester.Email;
                    obj.TicketStatus = TicketStatus.Open.ToString();
                    obj.Escalation = User.Identity.Name;
                    obj.CreatedBy = User.Identity.Name;
                    obj.CreatedDate = DateTime.Now;
                    obj.UpdatedBy = obj.CreatedBy;
                    obj.UpdatedDate = obj.CreatedDate;

                    string updatecommand = "Created";//GetUpdateCommand(m, obj);
                    var objHistory = SaveTicketHistory(obj, updatecommand);
                      
                    if (m.isClosed)
                    {
                        obj.TicketStatus = TicketStatus.Closed.ToString();
                        objHistory.TicketStatus = TicketStatus.Closed.ToString();
                    }

                    if (obj.IsValid && objHistory.IsValid)
                    {
                        obj = obj.Save();
                        objHistory = objHistory.Save();
                    }
                    else
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        foreach (var item in obj.GetBrokenRules())
                        {
                            sb.AppendLine(item.Description);
                        }
                        
                        foreach (var item in objHistory.GetBrokenRules())
                        {
                            sb.AppendLine(item.Description);
                        }
                        errorMessage = sb.ToString();
                    }

                    Customer cust = ViewModel.Ticket.ConvertToCustomerBusiness(m.Requester);

                    if (string.IsNullOrEmpty(cust.CreatedBy))
                    {
                        cust.CreatedBy = User.Identity.Name;
                        cust.CreatedDate = DateTime.Now;
                    }

                    cust.UpdatedBy = User.Identity.Name;
                    cust.UpdatedDate = DateTime.Now;

                    if (cust.IsValid)
                    {
                        cust = cust.Save();
                    }
                    else
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        foreach (var item in cust.GetBrokenRules())
                        {
                            sb.AppendLine(item.Description);
                        }
                        errorMessage = errorMessage + "---" + sb.ToString();
                    }

                    if (string.IsNullOrEmpty(errorMessage))
                    {
                        ctx.SaveChanges();
                        ViewBag.SuccessMsg = "Success";
                    }
                    else
                    {
                        throw new Exception("Terjadi kesalahan!");
                    }

                    
                }
                
               
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + "---" + errorMessage;
                ViewBag.ErrorMsg = errorMessage;

                LogActivity log = LogActivity.NewLogActivity();
                log.ControllerName = "Ticketing";
                log.ActionName = "Index";
                log.LogData = Newtonsoft.Json.JsonConvert.SerializeObject(errorMessage);
                log.CreatedBy = User.Identity.Name;
                log.CreatedDate = DateTime.Now;
                log = log.Save();
                return View(m);
            }

            return RedirectToAction("IndexInbox", "Inbox");
        }

        public ActionResult Update(string solution)
        {
            TicketSolution m = new TicketSolution();
            m.HowToSolving = solution;
            m.UpdatedBy = User.Identity.Name;
            m.UpdatedDate = DateTime.Now;

            return Json(m);
        }

        public ActionResult TicketOpenList()
        {
            var ticket = ISAT.Business.TicketInfoList.GetReportOpenTicketCurrentUser(User.Identity.Name);
            return View();
        }

        public ActionResult OpenTicket(string id)
        {
            Ticket ticket = Ticket.GetTicket(id);
            Customer cust = Customer.GetCustomer(ticket.Requester);
            if (cust == null)
            {
                cust = Customer.NewCustomer();
                return RedirectToAction("Index", "Ticketing", new { email = cust.Email, phoneNumber = cust.CustNo });
            }
            ViewModel.Ticket vm = new ViewModel.Ticket(ticket, cust);
            ViewBag.TicketHistory = TicketHistoryInfoList.GetTicketActivityByTicketNoAndUsername(ticket.TicketNo, User.Identity.Name);
            return View(vm);
        }

        public JsonResult getOpenTicket()
        {
            var ticket = ISAT.Business.TicketInfoList.GetReportOpenTicketCurrentUser(User.Identity.Name);
            return Json(ticket, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getOpenTicket(string sidx, string sord, int page, int rows)
        {
            var obj = ISAT.Business.TicketInfoList.GetReportOpenTicketCurrentUser(User.Identity.Name);
            var pageIndex = Convert.ToInt32(page) - 1;
            var pageSize = rows;
            var totalRecords = obj.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            //obj = obj.Skip(pageIndex * pageSize).Take(pageSize);

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = (
                  from l in obj
                  select new
                  {
                      i = l.idticket,
                      cell = new string[] {
                            l.idticket.ToString(),
                            l.TicketNo,
                            l.TicketSubject,
                            l.Requester,
                            l.CreatedDate.ToString(),
                            l.Priority,
                            l.TicketOwner
                      }
                  }).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClosedTicket(string id)
        {
            Ticket ticket = Ticket.GetTicket(id);
            Customer cust = Customer.GetCustomer(ticket.Requester);
            if (cust == null)
            {
                cust = Customer.NewCustomer();
                return RedirectToAction("Index", new { email = cust.Email, phoneNumber = cust.CustNo });
            }
            ViewModel.Ticket vm = new ViewModel.Ticket(ticket, cust);
            return View(vm);
        }

        [HttpPost]
        public ActionResult ClosedTicket(ViewModel.Ticket m)
        {
            try
            {
                Ticket obj = ViewModel.Ticket.ConvertToTicketBusiness(m);
                obj.Requester = m.Requester.Email;
                obj.TicketStatus = TicketStatus.Closed.ToString();
                obj.Escalation = User.Identity.Name;
                //obj.CreatedBy = User.Identity.Name;
                //obj.CreatedDate = DateTime.Now;
                obj.UpdatedBy = User.Identity.Name;
                obj.UpdatedDate = DateTime.Now;

                var objHistory = SaveTicketHistory(obj, "Closed");

                if (obj.IsValid && objHistory.IsValid)
                {
                    obj = obj.Save();
                    objHistory = objHistory.Save();
                }

                ViewBag.SuccessMsg = "Success";
            }
            catch (Exception ex)
            {
                return View(m);
            }

            return RedirectToAction("IndexInbox", "Inbox");
        }
        
        private TicketHistory SaveTicketHistory(Ticket obj, string command)
        {
            TicketHistory objHistory = TicketHistory.NewTicket();
            objHistory.TicketNo = obj.TicketNo;
            objHistory.TicketSubject = obj.TicketSubject;
            objHistory.CallerPosistion = obj.CallerPosistion;
            objHistory.Priority = obj.Priority;
            objHistory.TicketOwner = obj.TicketOwner;
            objHistory.TicketType = obj.TicketType;
            objHistory.TicketStatus = obj.TicketStatus;
            objHistory.TicketDescription = obj.TicketDescription;
            objHistory.Solution = obj.Solution;
            objHistory.Escalation = obj.Escalation;
            objHistory.Requester = obj.Requester;
            objHistory.TicketStatus = obj.TicketStatus;
            objHistory.Escalation = obj.Escalation;
            if (command == "Created")
            {
                objHistory.CreatedBy = obj.CreatedBy;
                objHistory.CreatedDate = obj.CreatedDate;
                objHistory.UpdatedBy = obj.CreatedBy;
                objHistory.UpdatedDate = obj.CreatedDate;
            }
            else
            {
                objHistory.CreatedBy = obj.CreatedBy;
                objHistory.CreatedDate = obj.CreatedDate;
                objHistory.UpdatedBy = User.Identity.Name;
                objHistory.UpdatedDate = DateTime.Now;
            }
            objHistory.UpdateCommand = command;
            return objHistory;
        }

        public ActionResult DisplayTicket(long ticketId)
        {
            Ticket ticket = Ticket.GetTicket(ticketId);
            Customer cust = Customer.GetCustomer(ticket.Requester);
            ViewModel.Ticket m = new ViewModel.Ticket(ticket, cust);
            var list = TicketHistoryInfoList.GetTicketActivityByTicketNo(ticket.TicketNo).ToList();
            
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

            list = list.Where(x => agenIdList.Contains(x.TicketOwner)).OrderByDescending(x => x.UpdatedDate).ToList();

            ViewBag.TicketHistory = list;
            return View(m);
        }

        public string GetUpdateCommand(ViewModel.Ticket ticket, Ticket obj)
        {
            string updatecommand = string.Empty;
            //created, add solution, escalated, closed
            if (ticket.IdTicket == 0)
            {
                updatecommand = "Created";
            }
            if (ticket.Solution != null && ticket.Solution.Count > 0)
            {
                string currentsolution = Newtonsoft.Json.JsonConvert.SerializeObject(ticket.Solution);
                if (!currentsolution.Equals(obj.Solution))
                {
                    updatecommand = "Add Solution";
                }
            }
            if (ticket.TicketStatus == "Closed")
            {
                updatecommand = "Closed";
            }
            return updatecommand;
        }

        //public JsonResult GetSubTicket(int id)
        //{
        //    TicketTypeDetailInfoList list = TicketTypeDetailInfoList.GetTicketTypeDetailInfoList(id);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}
    }

    public static class JsonExtensions
    {
        public static string ToJson(this Object obj)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return json;
        }
    }
}