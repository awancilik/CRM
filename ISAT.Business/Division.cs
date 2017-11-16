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
    public class Division : BusinessBase<Division>
    {
        #region Property

        public static readonly PropertyInfo<int> IdDivisionProperty = RegisterProperty<int>(c => c.IdDivision);
        public int IdDivision
        {
            get { return GetProperty(IdDivisionProperty); }
            private set { SetProperty(IdDivisionProperty, value); }
        }


        public static readonly PropertyInfo<string> DivisionNameProperty = RegisterProperty<string>(c => c.DivisionName);
        [Required]
        public string DivisionName
        {
            get { return GetProperty(DivisionNameProperty); }
            set { SetProperty(DivisionNameProperty, value); }
        }


        public static readonly PropertyInfo<string> DivisionDescProperty = RegisterProperty<string>(c => c.DivisionDesc);
        public string DivisionDesc
        {
            get { return GetProperty(DivisionDescProperty); }
            set { SetProperty(DivisionDescProperty, value); }
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

        public static Division NewDivision()
        {
            return DataPortal.Create<Division>();
        }

        public static Division GetDivision(int IdDivision)
        {
            Division obj = DataPortal.Fetch<Division>(IdDivision);
            if (obj.IdDivision == 0)
            {
                return null;
            }
            return obj;
        }

        public static Division GetDivision(SafeDataReader dr)
        {
            return DataPortal.Fetch<Division>(dr);
        }

        public static void DeleteDivision(int IdDivision)
        {
            DataPortal.Delete<Division>(IdDivision);
        }

        private Division()
        {

        }

        #endregion


        #region Data Access

        private void DataPortal_Fetch(int id)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT IdDivision, DivisionName, DivisionDesc, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM Division WHERE IdDivision = @IdDivision";
                cm.Parameters.AddWithValue("@IdDivision", id);
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
                cm.CommandText = "INSERT INTO Division(DivisionName, DivisionDesc, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate) VALUES (@DivisionName, @DivisionDesc, @CreatedBy, @CreatedDate, @UpdatedBy, @UpdatedDate);";
                DoCommand(cm);
            }
        }

        protected override void DataPortal_Update()
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateCommand();
                cm.CommandText = "UPDATE Division SET DivisionName=@DivisionName, DivisionDesc= @DivisionDesc, CreatedBy=@CreatedBy, CreatedDate=@CreatedDate, UpdatedBy=@UpdatedBy, UpdatedDate=@UpdatedDate WHERE IdDivision=@IdDivision";
                cm.Parameters.AddWithValue("@IdDivision", IdDivision);
                DoCommand(cm);
            }
        }

        protected override void DataPortal_DeleteSelf()
        {
            DataPortal_Delete(ReadProperty(IdDivisionProperty));
        }

        private void DataPortal_Delete(int id)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateCommand();
                cm.CommandText = "DELETE FROM Division WHERE IdDivision = @IdDivision";
                cm.Parameters.AddWithValue("@IdDivision", IdDivision);
                cm.ExecuteNonQuery();
            }
        }

        private void DataPortal_Fetch(SafeDataReader dr)
        {
            SetProperty(IdDivisionProperty, dr.GetInt32(0));
            SetProperty(DivisionNameProperty, dr.GetString(1));
            SetProperty(DivisionDescProperty, dr.GetString(2));
            SetProperty(CreatedByProperty, dr.GetString(3));
            SetProperty(CreatedDateProperty, dr.GetDateTime(4));
            SetProperty(UpdatedByProperty, dr.GetString(5));
            SetProperty(UpdatedDateProperty, dr.GetDateTime(6));
        }

        private int DoCommand(MySqlCommand cm)
        {
            int rowAffected = 0;

            cm.Parameters.AddWithValue("@DivisionName", DivisionName);
            cm.Parameters.AddWithValue("@DivisionDesc", DivisionDesc);
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
