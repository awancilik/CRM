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
    public class TicketTypeDetail : BusinessBase<TicketTypeDetail>
    {

        #region Property


        public static readonly PropertyInfo<int> idticketypedetailProperty = RegisterProperty<int>(c => c.idticketypedetail);
        public int idticketypedetail
        {
            get { return GetProperty(idticketypedetailProperty); }
            set { SetProperty(idticketypedetailProperty, value); }
        }


        public static readonly PropertyInfo<int> idTicketTypeProperty = RegisterProperty<int>(c => c.idTicketType);
        public int idTicketType
        {
            get { return GetProperty(idTicketTypeProperty); }
            set { SetProperty(idTicketTypeProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketTypeNameProperty = RegisterProperty<string>(c => c.TicketTypeName);
        public string TicketTypeName
        {
            get { return GetProperty(TicketTypeNameProperty); }
            set { SetProperty(TicketTypeNameProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketDescProperty = RegisterProperty<string>(c => c.TicketDesc);
        public string TicketDesc
        {
            get { return GetProperty(TicketDescProperty); }
            set { SetProperty(TicketDescProperty, value); }
        }


        public static readonly PropertyInfo<string> CreatedByProperty = RegisterProperty<string>(c => c.CreatedBy);
        public string CreatedBy
        {
            get { return GetProperty(CreatedByProperty); }
            set { SetProperty(CreatedByProperty, value); }
        }


        public static readonly PropertyInfo<DateTime> CreatedDateProperty = RegisterProperty<DateTime>(c => c.CreatedDate);
        public DateTime CreatedDate
        {
            get { return GetProperty(CreatedDateProperty); }
            set { SetProperty(CreatedDateProperty, value); }
        }


        public static readonly PropertyInfo<string> UpdatedByProperty = RegisterProperty<string>(c => c.UpdatedBy);
        public string UpdatedBy
        {
            get { return GetProperty(UpdatedByProperty); }
            set { SetProperty(UpdatedByProperty, value); }
        }


        public static readonly PropertyInfo<DateTime> UpdatedDateProperty = RegisterProperty<DateTime>(c => c.UpdatedDate);
        public DateTime UpdatedDate
        {
            get { return GetProperty(UpdatedDateProperty); }
            set { SetProperty(UpdatedDateProperty, value); }
        }



        #endregion


        #region Factory Method

        public static TicketTypeDetail NewTicketTypeDetail()
        {
            return DataPortal.Create<TicketTypeDetail>();
        }

        public static TicketTypeDetail GetTicketTypeDetail(int id)
        {
            return DataPortal.Fetch<TicketTypeDetail>(id);
        }

        public static TicketTypeDetail GetTicketTypeDetail(SafeDataReader dr)
        {
            return DataPortal.Fetch<TicketTypeDetail>(dr);
        }

        public static void DeleteTicketTypeDetail(int id)
        {
            DataPortal.Delete<TicketTypeDetail>(id);
        }

        private TicketTypeDetail()
        {

        }

        #endregion


        #region Data Access

        private void DataPortal_Fetch(int id)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT idticketypedetail, idTicketType, TicketTypeName, TicketDesc, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM tickettypedetail WHERE =@";
                cm.Parameters.AddWithValue("@", id);
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    DataPortal_Fetch(dr);
                }
            }
        }

        protected override void DataPortal_Insert()
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateCommand();
                cm.CommandText = "INSERT INTO tickettypedetail(idTicketType, TicketTypeName, TicketDesc, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate) VALUES ( @idTicketType, @TicketTypeName, @TicketDesc, @CreatedBy, @CreatedDate, @UpdatedBy, @UpdatedDate)";
                DoCommand(cm);
            }
        }

        protected override void DataPortal_Update()
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateCommand();
                cm.CommandText = "UPDATE tickettypedetail SET idTicketType=@idTicketType, TicketTypeName=@TicketTypeName, TicketDesc=@TicketDesc, CreatedBy=@CreatedBy, CreatedDate=@CreatedDate, UpdatedBy=@UpdatedBy, UpdatedDate=@UpdatedDate WHERE idticketypedetail=@idticketypedetail";
                cm.Parameters.AddWithValue("@idticketypedetail", idticketypedetail);
                DoCommand(cm);
            }
        }

        protected override void DataPortal_DeleteSelf()
        {
            DataPortal_Delete(ReadProperty(idticketypedetailProperty));
        }

        private void DataPortal_Delete(int id)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateCommand();
                cm.CommandText = "DELETE FROM tickettypedetail WHERE idticketypedetail=@idticketypedetail";
                cm.Parameters.AddWithValue("@", id);
                cm.ExecuteNonQuery();
            }
        }

        private void DataPortal_Fetch(SafeDataReader dr)
        {

            SetProperty(idticketypedetailProperty, dr.GetInt32(0));
            SetProperty(idTicketTypeProperty, dr.GetInt32(1));
            SetProperty(TicketTypeNameProperty, dr.GetString(2));
            SetProperty(TicketDescProperty, dr.GetString(3));
            SetProperty(CreatedByProperty, dr.GetString(4));
            SetProperty(CreatedDateProperty, dr.GetDateTime(5));
            SetProperty(UpdatedByProperty, dr.GetString(6));
            SetProperty(UpdatedDateProperty, dr.GetDateTime(7));

        }

        private int DoCommand(MySqlCommand cm)
        {
            int rowAffected = 0;

            cm.Parameters.AddWithValue("@idticketypedetail", idticketypedetail);
            cm.Parameters.AddWithValue("@idTicketType", idTicketType);
            cm.Parameters.AddWithValue("@TicketTypeName", TicketTypeName);
            cm.Parameters.AddWithValue("@TicketDesc", TicketDesc);
            cm.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            cm.Parameters.AddWithValue("@CreatedDate", CreatedDate);
            cm.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
            cm.Parameters.AddWithValue("@UpdatedDate", UpdatedDate);

            rowAffected = cm.ExecuteNonQuery();
            return rowAffected;
        }

        #endregion

    }
}
