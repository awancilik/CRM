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
    public class AgentInfoList : ReadOnlyBindingListBase<AgentInfoList, AgentInfo>
    {
         
        #region Factory Method

        public static AgentInfoList GetAgentInfoList()
        {
            return DataPortal.Fetch<AgentInfoList>();
        }

        public static AgentInfoList GetAgentByGroup(string groupName)
        {
            return DataPortal.Fetch<AgentInfoList>(groupName);
        }

        public static List<AgentInfo> GetAgentByEscalation(string groupName)
        {
            var list = GetAgentByGroup(groupName).ToList();
            list = list.Where(x => x.IsSupervisor).ToList();
            return list;
        }

        private AgentInfoList()
        {

        }

        #endregion


        #region Data Access

        private void DataPortal_Fetch()
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand() ;
                cm.CommandText = "SELECT idCustAgent, CustAgentName, AgentDesc, SPVID, GroupName, IsSupervisor, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM custagent";
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    AgentInfo info = AgentInfo.GetAgentInfo(dr);
                    Add(info);
                }
                cn.Close();
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch(string group)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = "SELECT idCustAgent, CustAgentName, AgentDesc, SPVID, GroupName, IsSupervisor, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM custagent WHERE GroupName=@group";
                cm.Parameters.AddWithValue("@group", group);
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    AgentInfo info = AgentInfo.GetAgentInfo(dr);
                    Add(info);
                }
                cn.Close();
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        #endregion


    }
}
