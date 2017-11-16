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
    public class AgentInfo : ReadOnlyBase<AgentInfo>
    {

        #region Property


        public static readonly PropertyInfo<string> idCustAgentProperty = RegisterProperty<string>(c => c.idCustAgent);
        public string idCustAgent
        {
            get { return GetProperty(idCustAgentProperty); }
            private set { LoadProperty(idCustAgentProperty, value); }
        }


        public static readonly PropertyInfo<string> CustAgentNameProperty = RegisterProperty<string>(c => c.CustAgentName);
        public string CustAgentName
        {
            get { return GetProperty(CustAgentNameProperty); }
            private set { LoadProperty(CustAgentNameProperty, value); }
        }


        public static readonly PropertyInfo<string> AgentDescProperty = RegisterProperty<string>(c => c.AgentDesc);
        public string AgentDesc
        {
            get { return GetProperty(AgentDescProperty); }
            private set { LoadProperty(AgentDescProperty, value); }
        }


        public static readonly PropertyInfo<string> SPVIDProperty = RegisterProperty<string>(c => c.SPVID);
        public string SPVID
        {
            get { return GetProperty(SPVIDProperty); }
            private set { LoadProperty(SPVIDProperty, value); }
        }


        public static readonly PropertyInfo<string> GroupNameProperty = RegisterProperty<string>(c => c.GroupName);
        public string GroupName
        {
            get { return GetProperty(GroupNameProperty); }
            private set { LoadProperty(GroupNameProperty, value); }
        }


        public static readonly PropertyInfo<bool> IsSupervisorProperty = RegisterProperty<bool>(c => c.IsSupervisor);
        public bool IsSupervisor
        {
            get
            {
                return GetProperty(IsSupervisorProperty);
            }
            private set
            {
                LoadProperty(IsSupervisorProperty, value);
            }
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

        public static AgentInfo GetAgentInfo(string idCustAgent)
        {
            return DataPortal.Fetch<AgentInfo>(idCustAgent);
        }

        public static AgentInfo GetAgentInfoSPV(string idCustAgent)
        {
            return DataPortal.Fetch<AgentInfo>(idCustAgent);
        }

        public static AgentInfo GetAgentInfo(SafeDataReader dr)
        {
            return DataPortal.Fetch<AgentInfo>(dr);
        }

        private AgentInfo()
        {

        }
        #endregion



        #region Data Access

        [Serializable()]
        private class CriteriaSPV : CriteriaBase<CriteriaSPV>
        {
            public static readonly PropertyInfo<int> IdProperty = RegisterProperty<int>(c => c.Id);
            public int Id
            {
                get { return ReadProperty(IdProperty); }
                set { LoadProperty(IdProperty, value); }
            }
        }


        private void DataPortal_Fetch(string id)
        {
            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = "SELECT idCustAgent, CustAgentName, AgentDesc, SPVID, GroupName, IsSupervisor, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM custagent WHERE idCustAgent=@idCustAgent";
                cm.Parameters.AddWithValue("@idCustAgent", id);
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    DataPortal_Fetch(dr);
                }
                cn.Close();
            }
        }

        private void DataPortal_Fetch(CriteriaSPV cr)
        {
            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = "SELECT idCustAgent, CustAgentName, AgentDesc, SPVID, GroupName, IsSupervisor, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM custagent WHERE SPVID=@SPVID AND IsSupervisor=true";
                cm.Parameters.AddWithValue("@SPVID", cr.Id);
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    DataPortal_Fetch(dr);
                }
                cn.Close();
            }
        }

        private void DataPortal_Fetch(SafeDataReader dr)
        {

            LoadProperty(idCustAgentProperty, dr.GetString(0));
            LoadProperty(CustAgentNameProperty, dr.GetString(1));
            LoadProperty(AgentDescProperty, dr.GetString(2));
            LoadProperty(SPVIDProperty, dr.GetString(3));
            LoadProperty(GroupNameProperty, dr.GetString(4));
            LoadProperty(IsSupervisorProperty, dr.GetBoolean(5));
            LoadProperty(CreatedByProperty, dr.GetString(6));
            LoadProperty(CreatedDateProperty, dr.GetDateTime(7));
            LoadProperty(UpdatedByProperty, dr.GetString(8));
            LoadProperty(UpdatedDateProperty, dr.GetDateTime(9));
        }

        #endregion

    }
}
