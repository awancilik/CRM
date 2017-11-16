using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Csla;
using Csla.Data;
using Csla.Security;
using Csla.Serialization;
using MySql.Data.MySqlClient;


namespace ISAT.Business
{
    [Serializable]
    public class Ticketing : CommandBase<Ticketing>
    {
        public static readonly PropertyInfo<Customer> CustObjectProperty = RegisterProperty<Customer>(c => c.CustObject);
        public Customer CustObject
        {
            get { return ReadProperty(CustObjectProperty); }
            private set { LoadProperty(CustObjectProperty, value); }
        }

        public static readonly PropertyInfo<Ticket> TicketObjectProperty = RegisterProperty<Ticket>(c => c.TicketObject);
        public Ticket TicketObject
        {
            get { return ReadProperty(TicketObjectProperty); }
            private set { LoadProperty(TicketObjectProperty, value); }
        }

        public static Ticketing Create(string agentId, string email)
        {
            Customer cust = Customer.NewCustomer();
            cust.Email = email;

            Ticket ticket = Ticket.NewTicket();
            ticket.Requester = email;
            ticket.TicketOwner = agentId;
            ticket.CreatedBy = agentId;
            ticket.CreatedDate = DateTime.Now;
            ticket.UpdatedBy = ticket.CreatedBy;
            ticket.UpdatedDate = ticket.CreatedDate;

            Ticketing cmd = new Ticketing() { CustObject = cust, TicketObject = ticket };
            cmd = DataPortal.Execute<Ticketing>(cmd);
            return cmd;
        }

        public void Update()
        {
            if (string.IsNullOrEmpty(this.TicketObject.TicketType))
            {
                TicketObject.TicketType = "-";
            }
            using (DatabaseManager ctx = new DatabaseManager(true))
            {

                if (TicketObject.TicketStatus != "Closed")
                {
                    TicketObject.TicketStatus = "Open";
                    if (string.IsNullOrEmpty(TicketObject.TicketNo))
                    {
                        TicketObject.TicketNo = TicketNo.Create(TicketObject.TicketOwner, "TICKET");
                    }
                }

                TicketObject.TicketNo = TicketNo.Create(TicketObject.TicketOwner, "TICKET");

                CustObject.ApplyEdit();
                TicketObject.ApplyEdit();

                TicketObject = TicketObject.Save();//submitted
                CustObject = CustObject.Save();


                ctx.SaveChanges();
            }


        }

        protected override void DataPortal_Execute()
        {
            Customer cust = null;
            Ticket newTicket = null;
            cust = Customer.GetCustomer(CustObject.Email);
            if (cust == null)
            {
                cust = Customer.NewCustomer();
                cust.Email = CustObject.Email;
                CustObject = cust;
            }
            else
            {
                CustObject = cust;
            }
            newTicket = Ticket.GetTicket(TicketObject.Requester);
            if (newTicket == null)
            {
                newTicket = Ticket.NewTicket();
                newTicket.Requester = cust.CustNo;
                newTicket.TicketOwner = TicketObject.TicketOwner;
                
                TicketObject = newTicket;
            }
            else
            {
                TicketObject = newTicket;
            }
        }
    }
}
