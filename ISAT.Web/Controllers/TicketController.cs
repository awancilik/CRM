using ISAT.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISAT.Web.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        // GET: Ticket
        public ActionResult IndexTicket()
        {
            return View();
        }

        public ActionResult Index(string phonenumber)
        {

            return View();
        }

        public ActionResult CreateTicket(string email)
        {
            string agentid = User.Identity.Name;
            Ticketing ticket = Ticketing.Create(agentid, email); 
            return View(ticket.TicketObject);
        }

        [HttpPost]
        public ActionResult CreateTicket(string name, string email, string phonenumber, string companyname, string pilihTiket,
                                        string TicketSubject, string description, string pilihPriority, string pilihEscalatedto)
        {
            var ticket = TicketNo.Create(User.Identity.Name, "TICKET");

            Ticketing tick = Ticketing.Create(User.Identity.Name, phonenumber);

            Ticket objTicket = tick.TicketObject;
            Customer cust = tick.CustObject;
            cust.FirstName = name;
            cust.Email = email;
            cust.CompanyName = companyname;
            objTicket.Priority = pilihPriority;
            if (pilihEscalatedto == "-")
            {
                objTicket.Escalation = User.Identity.Name;
            }
            else
            {
                objTicket.Escalation = pilihEscalatedto;
            }
            objTicket.TicketDescription = description;
            objTicket.TicketSubject = TicketSubject;
            objTicket.TicketOwner = User.Identity.Name;
            objTicket.CreatedBy = User.Identity.Name;
            objTicket.CreatedDate = DateTime.Now;
            objTicket.UpdatedBy = User.Identity.Name;
            objTicket.UpdatedDate = DateTime.Now;

            tick.Update();
            ViewBag.SuccessMsg = "Success";
            return View();
        }

        public ActionResult CallerProfile(string phoneNumber)
        {
            var obj = CustomerInfo.GetCustomerInfoByPhone(phoneNumber);
            return View(obj);
        }
    }
}