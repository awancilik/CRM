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
    public class TicketTypeDetailInfo : ReadOnlyBase<TicketTypeDetailInfo>
    {

        #region Property


        public static readonly PropertyInfo<int> idticketypedetailProperty = RegisterProperty<int>(c => c.idticketypedetail);
        public int idticketypedetail
        {
            get { return GetProperty(idticketypedetailProperty); }
            private set { LoadProperty(idticketypedetailProperty, value); }
        }


        public static readonly PropertyInfo<int> idTicketTypeProperty = RegisterProperty<int>(c => c.idTicketType);
        public int idTicketType
        {
            get { return GetProperty(idTicketTypeProperty); }
            private set { LoadProperty(idTicketTypeProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketTypeNameProperty = RegisterProperty<string>(c => c.TicketTypeName);
        public string TicketTypeName
        {
            get { return GetProperty(TicketTypeNameProperty); }
            private set { LoadProperty(TicketTypeNameProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketDescProperty = RegisterProperty<string>(c => c.TicketDesc);
        public string TicketDesc
        {
            get { return GetProperty(TicketDescProperty); }
            private set { LoadProperty(TicketDescProperty, value); }
        }


        public static readonly PropertyInfo<string> CreatedByProperty = RegisterProperty<string>(c => c.CreatedBy);
        public string CreatedBy
        {
            get { return GetProperty(CreatedByProperty); }
            private set { LoadProperty(CreatedByProperty, value); }
        }


        public static readonly PropertyInfo<DateTime> CreatedDateProperty = RegisterProperty<DateTime>(c => c.CreatedDate);
        public DateTime CreatedDate
        {
            get { return GetProperty(CreatedDateProperty); }
            private set { LoadProperty(CreatedDateProperty, value); }
        }


        public static readonly PropertyInfo<string> UpdatedByProperty = RegisterProperty<string>(c => c.UpdatedBy);
        public string UpdatedBy
        {
            get { return GetProperty(UpdatedByProperty); }
            private set { LoadProperty(UpdatedByProperty, value); }
        }


        public static readonly PropertyInfo<DateTime> UpdatedDateProperty = RegisterProperty<DateTime>(c => c.UpdatedDate);
        public DateTime UpdatedDate
        {
            get { return GetProperty(UpdatedDateProperty); }
            private set { LoadProperty(UpdatedDateProperty, value); }
        }



        #endregion


        #region Factory Method

        public static TicketTypeDetailInfo GetTicketTypeDetailInfo(int id)
        {
            return DataPortal.Fetch<TicketTypeDetailInfo>(id);
        }

        public static TicketTypeDetailInfo GetTicketTypeDetailInfo(SafeDataReader dr)
        {
            return DataPortal.Fetch<TicketTypeDetailInfo>(dr);
        }

        private TicketTypeDetailInfo()
        {

        }
        #endregion



        #region Data Access

        private void DataPortal_Fetch(int id)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT idticketypedetail, idTicketType, TicketTypeName, TicketDesc, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM tickettypedetail WHERE idticketypedetail=@idticketypedetail";
                cm.Parameters.AddWithValue("@idticketypedetail", id);
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    DataPortal_Fetch(dr);
                }
            }
        }

        private void DataPortal_Fetch(SafeDataReader dr)
        {

            LoadProperty(idticketypedetailProperty, dr.GetInt32(0));
            LoadProperty(idTicketTypeProperty, dr.GetInt32(1));
            LoadProperty(TicketTypeNameProperty, dr.GetString(2));
            LoadProperty(TicketDescProperty, dr.GetString(3));
            LoadProperty(CreatedByProperty, dr.GetString(4));
            LoadProperty(CreatedDateProperty, dr.GetDateTime(5));
            LoadProperty(UpdatedByProperty, dr.GetString(6));
            LoadProperty(UpdatedDateProperty, dr.GetDateTime(7));
        }

        #endregion

    }
}
