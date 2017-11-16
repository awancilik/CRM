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
    public class Agent : BusinessBase<Agent>
    {


        #region Property


        public static readonly PropertyInfo<string> idCustAgentProperty = RegisterProperty<string>(c => c.idCustAgent);
        public string idCustAgent
        {
            get { return GetProperty(idCustAgentProperty); }
            set { SetProperty(idCustAgentProperty, value); }
        }


        public static readonly PropertyInfo<string> CustAgentNameProperty = RegisterProperty<string>(c => c.CustAgentName);
        public string CustAgentName
        {
            get { return GetProperty(CustAgentNameProperty); }
            set { SetProperty(CustAgentNameProperty, value); }
        }


        public static readonly PropertyInfo<string> AgentDescProperty = RegisterProperty<string>(c => c.AgentDesc);
        public string AgentDesc
        {
            get { return GetProperty(AgentDescProperty); }
            set { SetProperty(AgentDescProperty, value); }
        }


        public static readonly PropertyInfo<string> SPVIDProperty = RegisterProperty<string>(c => c.SPVID);
        public string SPVID
        {
            get { return GetProperty(SPVIDProperty); }
            set { SetProperty(SPVIDProperty, value); }
        }


        public static readonly PropertyInfo<string> GroupNameProperty = RegisterProperty<string>(c => c.GroupName);
        public string GroupName
        {
            get { return GetProperty(GroupNameProperty); }
            set { SetProperty(GroupNameProperty, value); }
        }


        public static readonly PropertyInfo<bool> IsSupervisorProperty = RegisterProperty<bool>(c => c.IsSupervisor);
        public bool IsSupervisor
        {
            get
            {
                return GetProperty(IsSupervisorProperty);
            }
            set
            {
                SetProperty(IsSupervisorProperty, value);
            }
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

        public static Agent NewAgent()
        {
            return DataPortal.Create<Agent>();
        }

        public static Agent GetAgent(string idCustAgent)
        {
            return DataPortal.Fetch<Agent>(idCustAgent);
        }

        public static Agent GetAgent(SafeDataReader dr)
        {
            return DataPortal.Fetch<Agent>(dr);
        }

        public static void DeleteAgent(string idCustAgent)
        {
            DataPortal.Delete<Agent>(idCustAgent);
        }

        private Agent()
        {

        }

        #endregion


        #region Data Access

        private void DataPortal_Fetch(string id)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT idCustAgent, CustAgentName, AgentDesc, SPVID, GroupName, IsSupervisor, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM custagent WHERE idCustAgent=@idCustAgent";
                cm.Parameters.AddWithValue("@idCustAgent", id);
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
                cm.CommandText = "INSERT INTO custagent(idCustAgent, CustAgentName, AgentDesc, SPVID, GroupName, IsSupervisor, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate) VALUES (@idCustAgent, @CustAgentName, @AgentDesc, @SPVID, @GroupName, @IsSupervisor, @CreatedBy, @CreatedDate, @UpdatedBy, @UpdatedDate)";
                DoCommand(cm);
            }
        }

        protected override void DataPortal_Update()
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateCommand();
                cm.CommandText = "UPDATE custagent SET CustAgentName=@CustAgentName, AgentDesc=@AgentDesc, SPVID=@SPVID, GroupName=@GroupName, IsSupervisor=@IsSupervisor, CreatedBy=@CreatedBy, CreatedDate=@CreatedDate, UpdatedBy=@UpdatedBy, UpdatedDate=@UpdatedDate WHERE idCustAgent=@idCustAgent";
                cm.Parameters.AddWithValue("@idCustAgent", idCustAgent);
                DoCommand(cm);
            }
        }

        protected override void DataPortal_DeleteSelf()
        {
            DataPortal_Delete(ReadProperty(idCustAgentProperty));
        }

        private void DataPortal_Delete(string id)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateCommand();
                cm.CommandText = "DELETE FROM custagent WHERE idCustAgent=@idCustAgent";
                cm.Parameters.AddWithValue("@idCustAgent", idCustAgent);
                cm.ExecuteNonQuery();
            }
        }

        private void DataPortal_Fetch(SafeDataReader dr)
        {
            SetProperty(idCustAgentProperty, dr.GetString(0));
            SetProperty(CustAgentNameProperty, dr.GetString(1));
            SetProperty(AgentDescProperty, dr.GetString(2));
            SetProperty(SPVIDProperty, dr.GetString(3));
            SetProperty(GroupNameProperty, dr.GetString(4));
            SetProperty(IsSupervisorProperty, dr.GetBoolean(5));
            SetProperty(CreatedByProperty, dr.GetString(6));
            SetProperty(CreatedDateProperty, dr.GetDateTime(7));
            SetProperty(UpdatedByProperty, dr.GetString(8));
            SetProperty(UpdatedDateProperty, dr.GetDateTime(9));

        }

        private int DoCommand(MySqlCommand cm)
        {
            int rowAffected = 0;

            cm.Parameters.AddWithValue("@idCustAgent", idCustAgent);
            cm.Parameters.AddWithValue("@CustAgentName", CustAgentName);
            cm.Parameters.AddWithValue("@AgentDesc", AgentDesc);
            cm.Parameters.AddWithValue("@SPVID", SPVID);
            cm.Parameters.AddWithValue("@GroupName", GroupName);
            cm.Parameters.AddWithValue("@IsSupervisor", IsSupervisor);
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
