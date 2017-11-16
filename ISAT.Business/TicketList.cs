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
    public class TicketList : BusinessBindingListBase<TicketList, Ticket>
    {

        #region Factory Method

        public static TicketList NewTicketList()
        {
            return DataPortal.Create<TicketList>();
        }

        public static TicketList GetTicketList()
        {
            return DataPortal.Fetch<TicketList>();
        }

        private TicketList()
        {

        }

        #endregion
         
        #region Data Access

        private void DataPortal_Fetch()
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;

            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM ticket";
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    Ticket obj = Ticket.GetTicket(dr);
                    Add(obj);
                }
            }

            RaiseListChangedEvents = rlce;
        }

        #endregion

    }
}
