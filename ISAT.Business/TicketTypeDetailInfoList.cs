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
    public class TicketTypeDetailInfoList : ReadOnlyListBase<TicketTypeDetailInfoList, TicketTypeDetailInfo>
    {

        #region Factory Method

        public static TicketTypeDetailInfoList GetTicketTypeDetailInfoList()
        {
            return DataPortal.Fetch<TicketTypeDetailInfoList>();
        }

        public static TicketTypeDetailInfoList GetTicketTypeDetailInfoList(int parentId)
        {
            return DataPortal.Fetch<TicketTypeDetailInfoList>(parentId);
        }

        private TicketTypeDetailInfoList()
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
                cm.CommandText = "SELECT idticketypedetail, idTicketType, TicketTypeName, TicketDesc, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM tickettypedetail";
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    TicketTypeDetailInfo info = TicketTypeDetailInfo.GetTicketTypeDetailInfo(dr);
                    Add(info);
                }
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch(int id)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT idticketypedetail, idTicketType, TicketTypeName, TicketDesc, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM tickettypedetail WHERE idTicketType=@idTicketType";
                cm.Parameters.AddWithValue("@idTicketType", id);
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    TicketTypeDetailInfo info = TicketTypeDetailInfo.GetTicketTypeDetailInfo(dr);
                    Add(info);
                }
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        #endregion

    }
}
