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

namespace APILA.General
{
    [Serializable]
    public class Sequence : BusinessBase<Sequence>
    {
        #region Property

        public static readonly PropertyInfo<string> IdCustAgentProperty = RegisterProperty<string>(c => c.IdCustAgent);
        public string IdCustAgent
        {
            get { return GetProperty(IdCustAgentProperty); }
            set { SetProperty(IdCustAgentProperty, value); }
        }

        public static readonly PropertyInfo<string> CustAgentNameProperty = RegisterProperty<string>(c => c.CustAgentName);
        public string CustAgentName
        {
            get { return GetProperty(CustAgentNameProperty); }
            set { SetProperty(CustAgentNameProperty, value); }
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

        public static Sequence NewSequence()
        {
            return DataPortal.Create<Sequence>();
        }

        public static Sequence GetSequence(int id)
        {
            return DataPortal.Fetch<Sequence>(id);
        }

        public static void DeleteSequence(int id)
        {
            DataPortal.Delete<Sequence>(id);
        }

        private Sequence()
        {

        }

        #endregion

        #region Data Access

        [Serializable()]
        private class Criteria : CriteriaBase<Criteria>
        {
            public static readonly PropertyInfo<string> TableNameProperty = RegisterProperty<string>(c => c.TableName);
            public string TableName
            {
                get { return ReadProperty(TableNameProperty); }
                set { LoadProperty(TableNameProperty, value); }
            }
        }
         
        private void DataPortal_Fetch(Criteria id)
        {
            Database db = new Database();
            MySqlCommand cm = db.CreateSelectCommand();

        }

        protected override void DataPortal_Insert()
        {
            Database db = new Database(true);
            MySqlCommand cm = db.CreateCommand();
            cm.ExecuteNonQuery();
        }

        protected override void DataPortal_Update()
        {
            Database db = new Database(true);
            MySqlCommand cm = db.CreateCommand();
            cm.ExecuteNonQuery();
        }

        protected override void DataPortal_DeleteSelf()
        {
            throw new NotImplementedException();
        }

        private void DataPortal_Delete(Criteria id)
        {
            Database db = new Database(true);
            MySqlCommand cm = db.CreateCommand();
            cm.ExecuteNonQuery();
        }

        #endregion
    }
}
