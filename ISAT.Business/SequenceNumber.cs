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
    public class SequenceNumber : BusinessBase<SequenceNumber>
    {

        #region Property

        public static readonly PropertyInfo<string> TableNameProperty = RegisterProperty<string>(c => c.TableName);
        public string TableName
        {
            get { return GetProperty(TableNameProperty); }
            set { SetProperty(TableNameProperty, value); }
        }


        public static readonly PropertyInfo<string> PrefixProperty = RegisterProperty<string>(c => c.Prefix);
        public string Prefix
        {
            get { return GetProperty(PrefixProperty); }
            set { SetProperty(PrefixProperty, value); }
        }


        public static readonly PropertyInfo<int> SeqDateProperty = RegisterProperty<int>(c => c.SeqDate);
        public int SeqDate
        {
            get { return GetProperty(SeqDateProperty); }
            set { SetProperty(SeqDateProperty, value); }
        }


        public static readonly PropertyInfo<int> SeqNumberProperty = RegisterProperty<int>(c => c.SeqNumber);
        public int SeqNumber
        {
            get { return GetProperty(SeqNumberProperty); }
            set { SetProperty(SeqNumberProperty, value); }
        }



        #endregion


        #region Factory Method

        public static SequenceNumber NewSequenceNumber()
        {
            return DataPortal.Create<SequenceNumber>();
        }

        public static SequenceNumber GetSequenceNumber(string tableName, string prefix)
        {
            string dte = DateTime.Now.ToString("yyyyMMdd");
            SequenceNumber seq= DataPortal.Fetch<SequenceNumber>(new Criteria() { TableName = tableName, Prefix = prefix, SeqDate = Convert.ToInt32(dte) });
            if (string.IsNullOrEmpty(seq.TableName))
            {
                return null;
            }
            return seq;
        }

        public static SequenceNumber GetSequenceNumber(SafeDataReader dr)
        {
            return DataPortal.Fetch<SequenceNumber>(dr);
        }

        private SequenceNumber()
        {

        }

        #endregion


        #region Data Access

        private class Criteria
        {
            public string TableName { get; set; }
            public string Prefix { get; set; }
            public int SeqDate { get; set; }
        }

        private void DataPortal_Fetch(Criteria cr)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT TableName, Prefix, SeqDate, SeqNumber FROM sequencenumber WHERE TableName=@TableName AND Prefix=@Prefix ORDER BY SeqNumber desc LIMIT 1";
                cm.Parameters.AddWithValue("@TableName", cr.TableName);
                cm.Parameters.AddWithValue("@Prefix", cr.Prefix);
                cm.Parameters.AddWithValue("@SeqDate", cr.SeqDate);
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
                cm.CommandText = "INSERT INTO sequencenumber(TableName, Prefix, SeqDate, SeqNumber) VALUES (@TableName, @Prefix, @SeqDate, @SeqNumber)";
                DoCommand(cm);
            }
        }

        private void DataPortal_Fetch(SafeDataReader dr)
        {

            SetProperty(TableNameProperty, dr.GetString(0));
            SetProperty(PrefixProperty, dr.GetString(1));
            SetProperty(SeqDateProperty, dr.GetInt32(2));
            SetProperty(SeqNumberProperty, dr.GetInt32(3));

        }

        private int DoCommand(MySqlCommand cm)
        {
            int rowAffected = 0;

            cm.Parameters.AddWithValue("@TableName", TableName);
            cm.Parameters.AddWithValue("@Prefix", Prefix);
            cm.Parameters.AddWithValue("@SeqDate", SeqDate);
            cm.Parameters.AddWithValue("@SeqNumber", SeqNumber);

            rowAffected = cm.ExecuteNonQuery();
            return rowAffected;
        }

        #endregion

    }
}
