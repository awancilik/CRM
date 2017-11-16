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
    public class DivisionInfo : ReadOnlyBase<DivisionInfo>
    {
        #region Property


        public static readonly PropertyInfo<int> IdDivisionProperty = RegisterProperty<int>(c => c.IdDivision);
        public int IdDivision
        {
            get { return GetProperty(IdDivisionProperty); }
            private set { LoadProperty(IdDivisionProperty, value); }
        }


        public static readonly PropertyInfo<string> DivisionNameProperty = RegisterProperty<string>(c => c.DivisionName);
        [Required]
        public string DivisionName
        {
            get { return GetProperty(DivisionNameProperty); }
            set { LoadProperty(DivisionNameProperty, value); }
        }


        public static readonly PropertyInfo<string> DivisionDescProperty = RegisterProperty<string>(c => c.DivisionDesc);
        public string DivisionDesc
        {
            get { return GetProperty(DivisionDescProperty); }
            set { LoadProperty(DivisionDescProperty, value); }
        }


        public static readonly PropertyInfo<string> CreatedByProperty = RegisterProperty<string>(c => c.CreatedBy);
        [Required]
        public string CreatedBy
        {
            get { return GetProperty(CreatedByProperty); }
            set { LoadProperty(CreatedByProperty, value); }
        }


        public static readonly PropertyInfo<DateTime> CreatedDateProperty = RegisterProperty<DateTime>(c => c.CreatedDate);
        [Required]
        public DateTime CreatedDate
        {
            get { return GetProperty(CreatedDateProperty); }
            set { LoadProperty(CreatedDateProperty, value); }
        }


        public static readonly PropertyInfo<string> UpdatedByProperty = RegisterProperty<string>(c => c.UpdatedBy);
        [Required]
        public string UpdatedBy
        {
            get { return GetProperty(UpdatedByProperty); }
            set { LoadProperty(UpdatedByProperty, value); }
        }


        public static readonly PropertyInfo<DateTime> UpdatedDateProperty = RegisterProperty<DateTime>(c => c.UpdatedDate);
        [Required]
        public DateTime UpdatedDate
        {
            get { return GetProperty(UpdatedDateProperty); }
            set { LoadProperty(UpdatedDateProperty, value); }
        }

        #endregion


        #region Factory Method

        public static List<string> GetDivision()
        {
            List<string> list = new List<string>();
            var infoList = DivisionInfoList.GetDivisionInfoList();

            foreach (var item in infoList)
            {
                list.Add(item.DivisionName);
            }
            return list;
        }

        public static DivisionInfo GetDivisionInfo(int IdDivision)
        {
            DivisionInfo info = DataPortal.Fetch<DivisionInfo>(IdDivision);
            if (info.IdDivision == 0)
            {
                return null;
            }
            return info;
        }

        public static DivisionInfo GetDivisionInfo(SafeDataReader dr)
        {
            return DataPortal.Fetch<DivisionInfo>(dr);
        }

        private DivisionInfo()
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

        private void DataPortal_Fetch(SafeDataReader dr)
        {
            LoadProperty(IdDivisionProperty, dr.GetInt32(0));
            LoadProperty(DivisionNameProperty, dr.GetString(1));
            LoadProperty(DivisionDescProperty, dr.GetString(2));
            LoadProperty(CreatedByProperty, dr.GetString(3));
            LoadProperty(CreatedDateProperty, dr.GetDateTime(4));
            LoadProperty(UpdatedByProperty, dr.GetString(5));
            LoadProperty(UpdatedDateProperty, dr.GetDateTime(6));
        }

        #endregion
    }
}
