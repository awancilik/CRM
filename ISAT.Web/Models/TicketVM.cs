using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISAT.Web.Models
{
    public class TicketVM
    {
        public ISAT.Business.Ticket TicketModel { get; set; }
        public ISAT.Business.Customer CustomerModel { get; set; }
        public ISAT.Business.TicketSolutionList SolutionModel { get; set; }

        public TicketVM()
        {
            this.TicketModel = Business.Ticket.NewTicket();
            this.CustomerModel = Business.Customer.NewCustomer();
            this.SolutionModel = new Business.TicketSolutionList();
        }
    }
}