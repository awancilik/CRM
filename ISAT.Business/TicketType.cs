using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Csla;
using Csla.Data;
using Csla.Security;
using Csla.Serialization;
using MySql.Data.MySqlClient;


namespace ISAT.Business
{
    [Serializable]
    public class TicketType : BusinessBase<TicketType>
    {

        #region Property


        public static readonly PropertyInfo<int> idTicketTypeProperty = RegisterProperty<int>(c => c.idTicketType);
        public int idTicketType
        {
            get { return GetProperty(idTicketTypeProperty); }
            private set { SetProperty(idTicketTypeProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketTypeNameProperty = RegisterProperty<string>(c => c.TicketTypeName);
        [Required]
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
        [Required]
        public string CreatedBy
        {
            get { return GetProperty(CreatedByProperty); }
            set { SetProperty(CreatedByProperty, value); }
        }


        public static readonly PropertyInfo<DateTime> CreatedDateProperty = RegisterProperty<DateTime>(c => c.CreatedDate);
        [Required]
        public DateTime CreatedDate
        {
            get { return GetProperty(CreatedDateProperty); }
            set { SetProperty(CreatedDateProperty, value); }
        }


        public static readonly PropertyInfo<string> UpdatedByProperty = RegisterProperty<string>(c => c.UpdatedBy);
        [Required]
        public string UpdatedBy
        {
            get { return GetProperty(UpdatedByProperty); }
            set { SetProperty(UpdatedByProperty, value); }
        }


        public static readonly PropertyInfo<DateTime> UpdatedDateProperty = RegisterProperty<DateTime>(c => c.UpdatedDate);
        [Required]
        public DateTime UpdatedDate
        {
            get { return GetProperty(UpdatedDateProperty); }
            set { SetProperty(UpdatedDateProperty, value); }
        }



        #endregion


        #region Factory Method

        public static TicketType NewTicketType()
        {
            return DataPortal.Create<TicketType>();
        }

        public static TicketType GetTicketType(int idTicketType)
        {
            TicketType obj = DataPortal.Fetch<TicketType>(idTicketType);
            if (obj.idTicketType ==0)
            {
                return null;
            }
            return obj;
        }

        public static TicketType GetTicketType(SafeDataReader dr)
        {
            return DataPortal.Fetch<TicketType>(dr);
        }

        public static void DeleteTicketType(int idTicketType)
        {
            DataPortal.Delete<TicketType>(idTicketType);
        }

        private TicketType()
        {

        }

        #endregion


        #region Data Access

        private void DataPortal_Fetch(int id)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT idTicketType, TicketTypeName, TicketDesc, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM tickettype WHERE idTicketType=@idTicketType";
                cm.Parameters.AddWithValue("@idTicketType", id);
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
                cm.CommandText = "INSERT INTO tickettype(TicketTypeName, TicketDesc, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate) VALUES (@TicketTypeName, @TicketDesc, @CreatedBy, @CreatedDate, @UpdatedBy, @UpdatedDate);";
                DoCommand(cm);
                cm.CommandText = "SELECT LAST_INSERT_ID() FROM Dual";
                SetProperty(idTicketTypeProperty, Convert.ToInt32(cm.ExecuteScalar()));
            }
        }

        protected override void DataPortal_Update()
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateCommand();
                cm.CommandText = "UPDATE tickettype SET TicketTypeName=@TicketTypeName, TicketDesc=@TicketDesc, CreatedBy=@CreatedBy, CreatedDate=@CreatedDate, UpdatedBy=@UpdatedBy, UpdatedDate=@UpdatedDate WHERE idTicketType=@idTicketType";
                cm.Parameters.AddWithValue("@idTicketType", idTicketType);
                DoCommand(cm);
            }
        }

        protected override void DataPortal_DeleteSelf()
        {
            DataPortal_Delete(ReadProperty(idTicketTypeProperty));
        }

        private void DataPortal_Delete(int id)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateCommand();
                cm.CommandText = "DELETE FROM tickettype WHERE idTicketType=@idTicketType";
                cm.Parameters.AddWithValue("@idTicketType", idTicketType);
                cm.ExecuteNonQuery();
            }
        }

        private void DataPortal_Fetch(SafeDataReader dr)
        { 
            SetProperty(idTicketTypeProperty, dr.GetInt32(0));
            SetProperty(TicketTypeNameProperty, dr.GetString(1));
            SetProperty(TicketDescProperty, dr.GetString(2));
            SetProperty(CreatedByProperty, dr.GetString(3));
            SetProperty(CreatedDateProperty, dr.GetDateTime(4));
            SetProperty(UpdatedByProperty, dr.GetString(5));
            SetProperty(UpdatedDateProperty, dr.GetDateTime(6));

        }

        private int DoCommand(MySqlCommand cm)
        {
            int rowAffected = 0;

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
