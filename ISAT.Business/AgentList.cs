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
    public class AgentList : BusinessBindingListBase<AgentList, Agent>
    {
         
        #region Factory Method

        public static AgentList NewAgentList()
        {
            return DataPortal.Create<AgentList>();
        }

        public static AgentList GetAgentList()
        {
            return DataPortal.Fetch<AgentList>();
        }

        private AgentList()
        {

        }

        #endregion 

        #region Data Access

        private void DataPortal_Fetch()
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;

            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT idCustAgent, CustAgentName, AgentDesc, SPVID, GroupName, IsSupervisor, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM custagent";
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    Agent obj = Agent.GetAgent(dr);
                    Add(obj);
                }
            }

            RaiseListChangedEvents = rlce;
        }

        #endregion
         
    }
}
