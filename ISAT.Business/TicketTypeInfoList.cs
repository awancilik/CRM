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
    public class TicketTypeInfoList : ReadOnlyBindingListBase<TicketTypeInfoList, TicketTypeInfo>
    {

        #region Factory Method

        public static TicketTypeInfoList GetTicketTypeInfoList()
        {
            return DataPortal.Fetch<TicketTypeInfoList>();
        }

        private TicketTypeInfoList()
        {

        }

        #endregion


        #region Data Access

        private void DataPortal_Fetch()
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT idTicketType, TicketTypeName, TicketDesc, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM tickettype";
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    TicketTypeInfo info = TicketTypeInfo.GetTicketTypeInfo(dr);
                    Add(info);
                }
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        #endregion

    }
}
