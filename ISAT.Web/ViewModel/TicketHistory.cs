using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISAT.Web.ViewModel
{
    public class TicketHistory
    {
        public long IdTicket { get; set; }
        public string TicketNo { get; set; }
        public string TicketSubject { get; set; }
        public Customer Requester { get; set; }
        public string CallerPosition { get; set; }
        public string Priority { get; set; }
        public string TicketOwner { get; set; }
        public string TicketType { get; set; }
        public string TicketStatus { get; set; }
        public string OpenDate { get; set; }
        public string ClosedDate { get; set; }
        public string TicketDesc { get; set; }
        public List<TicketSolution> Solution { get; set; }
        public string Escalation { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        public bool isClosed { get; set; }

        public TicketHistory()
        {
            Solution = new List<TicketSolution>();
        }

        public TicketHistory(ISAT.Business.TicketHistory ticket, ISAT.Business.Customer cust)
        {
            this.IdTicket = ticket.idticket;
            this.TicketNo = ticket.TicketNo;
            this.TicketSubject = ticket.TicketSubject;

            Customer newcust = new Customer()
            {
                IdCustomer = cust.idCustomer,
                Custno = cust.CustNo,
                FirstName = cust.FirstName,
                LastName = cust.LastName,
                Email = cust.Email,
                CompanyName = cust.CompanyName,
                CreatedBy = cust.CreatedBy,
                CreatedDate = cust.CreatedDate,
                UpdatedBy = cust.UpdatedBy,
                UpdatedDate = cust.UpdatedDate
            };

            this.Requester = newcust;
            this.CallerPosition = ticket.CallerPosistion;
            this.Priority = ticket.Priority;
            this.TicketOwner = ticket.TicketOwner;
            this.TicketType = ticket.TicketType;
            this.TicketStatus = ticket.TicketStatus;
            this.TicketDesc = ticket.TicketDescription;

            this.Solution = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TicketSolution>>(ticket.Solution);
            if (this.Solution == null)
            {
                this.Solution = new List<TicketSolution>();
            }
            this.Escalation = ticket.Escalation;
            this.CreatedBy = ticket.CreatedBy;
            this.CreatedDate = ticket.CreatedDate;
            this.UpdatedBy = ticket.UpdatedBy;
            this.UpdatedDate = ticket.UpdatedDate;
        }

        public static ISAT.Business.TicketHistory ConvertToTicketBusiness(TicketHistory m)
        {
            ISAT.Business.TicketHistory obj = ISAT.Business.TicketHistory.GetTicket(m.TicketNo);
            if (obj == null)
            {
                obj = ISAT.Business.TicketHistory.NewTicket();
            }

            obj.TicketNo = m.TicketNo;
            obj.TicketSubject = m.TicketSubject;
            obj.CallerPosistion = m.CallerPosition;
            obj.Priority = m.Priority;
            obj.TicketOwner = m.TicketOwner;
            obj.TicketType = m.TicketType;
            obj.TicketStatus = m.TicketStatus;
            obj.TicketDescription = m.TicketDesc;
            obj.Solution = Newtonsoft.Json.JsonConvert.SerializeObject(m.Solution);
            obj.Escalation = m.Escalation;
            obj.CreatedBy = m.CreatedBy;
            obj.CreatedDate = m.CreatedDate;
            obj.UpdatedBy = m.UpdatedBy;
            obj.UpdatedDate = m.UpdatedDate;

            return obj;
        }

        public static ISAT.Business.Customer ConvertToCustomerBusiness(Customer m)
        {
            ISAT.Business.Customer obj = ISAT.Business.Customer.GetCustomer(m.Email);
            if (obj == null)
            {
                obj = ISAT.Business.Customer.NewCustomer();
                obj.CustNo = m.Custno;
                obj.FirstName = m.FirstName;
                obj.LastName = m.LastName;
                obj.Email = m.Email;
                obj.CompanyName = m.CompanyName;
                obj.CreatedBy = m.CreatedBy;
                obj.CreatedDate = m.CreatedDate;
                obj.UpdatedBy = m.UpdatedBy;
                obj.UpdatedDate = m.UpdatedDate;
            }
            return obj;
        }
    }
}