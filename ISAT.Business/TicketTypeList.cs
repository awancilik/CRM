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
    public class TicketTypeList : BusinessBindingListBase<TicketTypeList, TicketType>
    {

        #region Factory Method

        public static TicketTypeList NewTicketTypeList()
        {
            return DataPortal.Create<TicketTypeList>();
        }

        public static TicketTypeList GetTicketTypeList()
        {
            return DataPortal.Fetch<TicketTypeList>();
        }

        private TicketTypeList()
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
                cm.CommandText = "SELECT idTicketType, TicketTypeName, TicketDesc, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM tickettype";
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    TicketType obj = TicketType.GetTicketType(dr);
                    Add(obj);
                }
            }

            RaiseListChangedEvents = rlce;
        }

        #endregion

    }
}
