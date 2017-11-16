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
    public class DivisionList : BusinessBindingListBase<DivisionList, Division>
    {
        #region Factory Method

        public static DivisionList NewDivisionList()
        {
            return DataPortal.Create<DivisionList>();
        }

        public static DivisionList GetDivisionList()
        {
            return DataPortal.Fetch<DivisionList>();
        }

        private DivisionList()
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
                cm.CommandText = "SELECT IdDivision, DivisionName, DivisionDesc, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM Division";
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    Division obj = Division.GetDivision(dr);
                    Add(obj);
                }
            }

            RaiseListChangedEvents = rlce;
        }

        #endregion
    }
}
