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
    public class CustomerInfoList : ReadOnlyBindingListBase<CustomerInfoList, CustomerInfo>
    {

        #region Factory Method

        public static CustomerInfoList GetCustomerInfoList()
        {
            return DataPortal.Fetch<CustomerInfoList>();
        }

        private CustomerInfoList()
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
                cm.CommandText = "SELECT idCustomer, CustNo, FirstName, LastName, Email, CompanyName, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM customer";
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    CustomerInfo info = CustomerInfo.GetCustomerInfo(dr);
                    Add(info);
                }
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        #endregion

    }
}
