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
    public class CustomerInfo : ReadOnlyBase<CustomerInfo>
    {

        #region Property


        public static readonly PropertyInfo<long> idCustomerProperty = RegisterProperty<long>(c => c.idCustomer);
        public long idCustomer
        {
            get { return GetProperty(idCustomerProperty); }
            private set { LoadProperty(idCustomerProperty, value); }
        }


        public static readonly PropertyInfo<string> CustNoProperty = RegisterProperty<string>(c => c.CustNo);
        public string CustNo
        {
            get { return GetProperty(CustNoProperty); }
            private set { LoadProperty(CustNoProperty, value); }
        }


        public static readonly PropertyInfo<string> FirstNameProperty = RegisterProperty<string>(c => c.FirstName);
        public string FirstName
        {
            get { return GetProperty(FirstNameProperty); }
            private set { LoadProperty(FirstNameProperty, value); }
        }


        public static readonly PropertyInfo<string> LastNameProperty = RegisterProperty<string>(c => c.LastName);
        public string LastName
        {
            get { return GetProperty(LastNameProperty); }
            private set { LoadProperty(LastNameProperty, value); }
        }


        public static readonly PropertyInfo<string> EmailProperty = RegisterProperty<string>(c => c.Email);
        public string Email
        {
            get { return GetProperty(EmailProperty); }
            private set { LoadProperty(EmailProperty, value); }
        }


        public static readonly PropertyInfo<string> CompanyNameProperty = RegisterProperty<string>(c => c.CompanyName);
        public string CompanyName
        {
            get { return GetProperty(CompanyNameProperty); }
            private set { LoadProperty(CompanyNameProperty, value); }
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

        public static CustomerInfo GetCustomerInfo(int idCustomer)
        {
            CustomerInfo info = DataPortal.Fetch<CustomerInfo>(idCustomer);
            if (info.idCustomer == 0)
            {
                return null;
            }
            return info;
        }

        public static CustomerInfo GetCustomerInfo(string email)
        {
            CustomerInfo info = DataPortal.Fetch<CustomerInfo>(email);
            if (info.idCustomer == 0)
            {
                return null;
            }
            return info;
        }

        public static CustomerInfo GetCustomerInfoByPhone(string phone)
        {
            CustomerInfo info = DataPortal.Fetch<CustomerInfo>(new CriteriaPhone() { PhoneNumber=phone });
            if (info.idCustomer == 0)
            {
                return null;
            }
            return info;
        }

        public static CustomerInfo GetCustomerInfo(SafeDataReader dr)
        {
            return DataPortal.Fetch<CustomerInfo>(dr);
        }

        private CustomerInfo()
        {

        }

        #endregion



        #region Data Access

        [Serializable()]
        private class CriteriaPhone : CriteriaBase<CriteriaPhone>
        {
            public static readonly PropertyInfo<string> PhoneNumberProperty = RegisterProperty<string>(c => c.PhoneNumber);
            public string PhoneNumber
            {
                get { return ReadProperty(PhoneNumberProperty); }
                set { LoadProperty(PhoneNumberProperty, value); }
            }
        }


        private void DataPortal_Fetch(int id)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT idCustomer, CustNo, FirstName, LastName, Email, CompanyName, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM customer WHERE idCustomer=@idCustomer";
                cm.Parameters.AddWithValue("@idCustomer", id);
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    DataPortal_Fetch(dr);
                }
            }
        }


        private void DataPortal_Fetch(string id)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT idCustomer, CustNo, FirstName, LastName, Email, CompanyName, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM customer WHERE Email=@Email";
                cm.Parameters.AddWithValue("@Email", id);
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    DataPortal_Fetch(dr);
                }
            }
        }

        private void DataPortal_Fetch(CriteriaPhone cr)
        {
            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = "SELECT idCustomer, CustNo, FirstName, LastName, Email, CompanyName, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM customer WHERE CustNo=@CustNo";
                cm.Parameters.AddWithValue("@CustNo", cr.PhoneNumber);
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
            LoadProperty(idCustomerProperty, dr.GetInt64(0));
            LoadProperty(CustNoProperty, dr.GetString(1));
            LoadProperty(FirstNameProperty, dr.GetString(2));
            LoadProperty(LastNameProperty, dr.GetString(3));
            LoadProperty(EmailProperty, dr.GetString(4));
            LoadProperty(CompanyNameProperty, dr.GetString(5));
            LoadProperty(CreatedByProperty, dr.GetString(6));
            LoadProperty(CreatedDateProperty, dr.GetDateTime(7));
            LoadProperty(UpdatedByProperty, dr.GetString(8));
            LoadProperty(UpdatedDateProperty, dr.GetDateTime(9));
        }

        #endregion

    }
}
