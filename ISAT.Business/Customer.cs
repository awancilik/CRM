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
    public class Customer : BusinessBase<Customer>
    { 
        #region Property
         
        public static readonly PropertyInfo<long> idCustomerProperty = RegisterProperty<long>(c => c.idCustomer);
        public long idCustomer
        {
            get { return GetProperty(idCustomerProperty); }
            private set { SetProperty(idCustomerProperty, value); }
        }


        public static readonly PropertyInfo<string> CustNoProperty = RegisterProperty<string>(c => c.CustNo); 
        public string CustNo
        {
            get { return GetProperty(CustNoProperty); }
            set { SetProperty(CustNoProperty, value); }
        }


        public static readonly PropertyInfo<string> FirstNameProperty = RegisterProperty<string>(c => c.FirstName);
        [Required]
        public string FirstName
        {
            get { return GetProperty(FirstNameProperty); }
            set { SetProperty(FirstNameProperty, value); }
        }


        public static readonly PropertyInfo<string> LastNameProperty = RegisterProperty<string>(c => c.LastName);
        public string LastName
        {
            get { return GetProperty(LastNameProperty); }
            set { SetProperty(LastNameProperty, value); }
        }


        public static readonly PropertyInfo<string> EmailProperty = RegisterProperty<string>(c => c.Email);
        [Required]
        public string Email
        {
            get { return GetProperty(EmailProperty); }
            set { SetProperty(EmailProperty, value); }
        }


        public static readonly PropertyInfo<string> CompanyNameProperty = RegisterProperty<string>(c => c.CompanyName); 
        public string CompanyName
        {
            get { return GetProperty(CompanyNameProperty); }
            set { SetProperty(CompanyNameProperty, value); }
        }


        public static readonly PropertyInfo<string> CreatedByProperty = RegisterProperty<string>(c => c.CreatedBy);
        [Required]
        public string CreatedBy
        {
            get { return GetProperty(CreatedByProperty); }
            set { SetProperty(CreatedByProperty, value); }
        }


        public static readonly PropertyInfo<DateTime> CreatedDateProperty = RegisterProperty<DateTime>(c => c.CreatedDate);
        [Required]
        public DateTime CreatedDate
        {
            get { return GetProperty(CreatedDateProperty); }
            set { SetProperty(CreatedDateProperty, value); }
        }


        public static readonly PropertyInfo<string> UpdatedByProperty = RegisterProperty<string>(c => c.UpdatedBy);
        [Required]
        public string UpdatedBy
        {
            get { return GetProperty(UpdatedByProperty); }
            set { SetProperty(UpdatedByProperty, value); }
        }


        public static readonly PropertyInfo<DateTime> UpdatedDateProperty = RegisterProperty<DateTime>(c => c.UpdatedDate);
        [Required]
        public DateTime UpdatedDate
        {
            get { return GetProperty(UpdatedDateProperty); }
            set { SetProperty(UpdatedDateProperty, value); }
        }

        public static readonly PropertyInfo<string> CallerPosistionProperty = RegisterProperty<string>(c => c.CallerPosistion);
        public string CallerPosistion
        {
            get { return GetProperty(CallerPosistionProperty); }
            set { SetProperty(CallerPosistionProperty, value); }
        }

        #endregion

        #region Factory Method

        public static Customer NewCustomer()
        {
            return DataPortal.Create<Customer>();
        }

        public static Customer GetCustomer(long idCustomer)
        {
            Customer obj= DataPortal.Fetch<Customer>(idCustomer);
            if (obj.idCustomer==0)
            {
                return null;
            }
            return obj;
        }

        public static Customer GetCustomer(string email)
        {
            Customer obj = DataPortal.Fetch<Customer>(email);
            if (obj.idCustomer == 0)
            {
                return null;
            }
            return obj;
        }

        public static Customer GetCustomer(SafeDataReader dr)
        {
            return DataPortal.Fetch<Customer>(dr);
        }

        public static void DeleteCustomer(long idCustomer)
        {
            DataPortal.Delete<Customer>(idCustomer);
        }

        private Customer()
        {

        }

        #endregion
         
        #region Data Access

        private void DataPortal_Fetch(long id)
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

        private void DataPortal_Fetch(string email)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT idCustomer, CustNo, FirstName, LastName, Email, CompanyName, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM customer WHERE Email=@Email";
                cm.Parameters.AddWithValue("@Email", email);
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
                cm.CommandText = "INSERT INTO customer(CustNo, FirstName, LastName, Email, CompanyName, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate) VALUES (@CustNo, @FirstName, @LastName, @Email, @CompanyName, @CreatedBy, @CreatedDate, @UpdatedBy, @UpdatedDate)";
                DoCommand(cm);
            }
        }

        protected override void DataPortal_Update()
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateCommand();
                cm.CommandText = "UPDATE customer SET CustNo=@CustNo, FirstName=@FirstName, LastName=@LastName, Email=@Email, CompanyName=@CompanyName, CreatedBy=@CreatedBy, CreatedDate=@CreatedDate, UpdatedBy=@UpdatedBy, UpdatedDate=@UpdatedDate WHERE idCustomer=@idCustomer";
                cm.Parameters.AddWithValue("@idCustomer", idCustomer);
                DoCommand(cm);
            }
        }

        protected override void DataPortal_DeleteSelf()
        {
            DataPortal_Delete(ReadProperty(idCustomerProperty));
        }

        private void DataPortal_Delete(long id)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateCommand();
                cm.CommandText = "DELETE FROM customer WHERE idCustomer=@idCustomer";
                cm.Parameters.AddWithValue("@idCustomer", id);
                cm.ExecuteNonQuery();
            }
        }

        private void DataPortal_Fetch(SafeDataReader dr)
        {
            SetProperty(idCustomerProperty, dr.GetInt64(0));
            SetProperty(CustNoProperty, dr.GetString(1));
            SetProperty(FirstNameProperty, dr.GetString(2));
            SetProperty(LastNameProperty, dr.GetString(3));
            SetProperty(EmailProperty, dr.GetString(4));
            SetProperty(CompanyNameProperty, dr.GetString(5));
            SetProperty(CreatedByProperty, dr.GetString(6));
            SetProperty(CreatedDateProperty, dr.GetDateTime(7));
            SetProperty(UpdatedByProperty, dr.GetString(8));
            SetProperty(UpdatedDateProperty, dr.GetDateTime(9));

        }

        private int DoCommand(MySqlCommand cm)
        {
            int rowAffected = 0;

            cm.Parameters.AddWithValue("@CustNo", CustNo);
            cm.Parameters.AddWithValue("@FirstName", FirstName);
            cm.Parameters.AddWithValue("@LastName", LastName);
            cm.Parameters.AddWithValue("@Email", Email);
            cm.Parameters.AddWithValue("@CompanyName", CompanyName);
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
