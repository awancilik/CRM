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
    public class DivisionInfoList : ReadOnlyBindingListBase<DivisionInfoList, DivisionInfo>
    {
        #region Factory Method

        public static DivisionInfoList GetDivisionInfoList()
        {
            return DataPortal.Fetch<DivisionInfoList>();
        }

        private DivisionInfoList()
        {

        }

        #endregion


        #region Data Access

        private void DataPortal_Fetch()
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT IdDivision, DivisionName, DivisionDesc, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM Division";
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    DivisionInfo info = DivisionInfo.GetDivisionInfo(dr);
                    Add(info);
                }
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        #endregion
    }
}
