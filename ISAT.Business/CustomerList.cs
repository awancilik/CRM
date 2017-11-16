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
    public class CustomerList : BusinessBindingListBase<CustomerList, Customer>
    {

        #region Factory Method

        public static CustomerList NewCustomerList()
        {
            return DataPortal.Create<CustomerList>();
        }

        public static CustomerList GetCustomerList()
        {
            return DataPortal.Fetch<CustomerList>();
        }

        private CustomerList()
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
                cm.CommandText = "SELECT idCustomer, CustNo, FirstName, LastName, Email, CompanyName, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM customer";
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    Customer obj = Customer.GetCustomer(dr);
                    Add(obj);
                }
            }

            RaiseListChangedEvents = rlce;
        }

        #endregion

    }
}
