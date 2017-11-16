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
    public class TicketHistoryInfoList : ReadOnlyBindingListBase<TicketHistoryInfoList, TicketHistoryInfo>
    {

        #region Factory Method

        public static TicketHistoryInfoList GetTicketHistoryInfoList()
        {
            return DataPortal.Fetch<TicketHistoryInfoList>();
        }

        public static TicketHistoryInfoList GetTicketActivity(string custno)
        {
            return DataPortal.Fetch<TicketHistoryInfoList>(new CriteriaActivity() { Email = custno });
        }

        public static TicketHistoryInfoList GetTicketActivityByTicketNo(string ticketNo)
        {
            return DataPortal.Fetch<TicketHistoryInfoList>(new CriteriaActivityTicket() { TIcketNo = ticketNo });
        }

        public static TicketHistoryInfoList GetTicketActivityByTicketNoAndUsername(string ticketNo, string username)
        {
            return DataPortal.Fetch<TicketHistoryInfoList>(new CriteriaActivityTicketByNoTiketAndUser() { TicketNo = ticketNo , UserName = username });
        }

        public static TicketHistoryInfoList GetTicketInbox(string username)
        {
            return DataPortal.Fetch<TicketHistoryInfoList>(username);
        }

        public static TicketHistoryInfoList GetReportOpenTicket()
        {
            return DataPortal.Fetch<TicketHistoryInfoList>(new CriteriaOpenTicket());
        }

        public static TicketHistoryInfoList GetReportOpenTicketCurrentUser(string username)
        {
            return DataPortal.Fetch<TicketHistoryInfoList>(new CriteriaOpenTicketByUser() { UserName = username });
        }

        public static TicketHistoryInfoList GetReportByDate(DateTime startDate, DateTime endDate)
        {
            return DataPortal.Fetch<TicketHistoryInfoList>(new CriteriaByDate() { StartDate = startDate, EndDate = endDate });
        }

        public static TicketHistoryInfoList GetReportCloseTicket()
        {
            return DataPortal.Fetch<TicketHistoryInfoList>(new CriteriaCloseTicket());
        }

        public static TicketHistoryInfoList GetReportCloseTicketCurrentUser(string username)
        {
            return DataPortal.Fetch<TicketHistoryInfoList>(new CriteriaCloseTicketByUser() { UserName = username });
        }

        private TicketHistoryInfoList()
        {

        }

        #endregion


        #region Data Access

        [Serializable()]
        private class CriteriaOpenTicket : CriteriaBase<CriteriaOpenTicket>
        {

        }

        [Serializable()]
        private class CriteriaCloseTicket : CriteriaBase<CriteriaCloseTicket>
        {

        }

        [Serializable()]
        private class CriteriaByDate : CriteriaBase<CriteriaByDate>
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        [Serializable()]
        private class CriteriaOpenTicketByUser : CriteriaBase<CriteriaOpenTicket>
        {
            public string UserName { get; set; }
        }

        [Serializable()]
        private class CriteriaCloseTicketByUser : CriteriaBase<CriteriaCloseTicket>
        {
            public string UserName { get; set; }
        }

        [Serializable()]
        private class CriteriaActivity : CriteriaBase<CriteriaActivity>
        {
            public string Email { get; set; }
        }

        [Serializable()]
        private class CriteriaActivityTicket : CriteriaBase<CriteriaActivityTicket>
        {
            public string TIcketNo { get; set; }
        }

        [Serializable()]
        private class CriteriaActivityTicketByNoTiketAndUser : CriteriaBase<CriteriaActivityTicketByNoTiketAndUser>
        {
            public string TicketNo { get; set; }
            public string UserName { get; set; }
        }

        private void DataPortal_Fetch(CriteriaOpenTicketByUser criteria)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = @"SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, 
                                    CreatedDate, UpdatedBy, UpdatedDate, UpdateCommand FROM ticket_his 
                                    WHERE Escalation=@Escalation AND TicketStatus=@TicketStatus";
                cm.Parameters.AddWithValue("@Escalation", criteria.UserName);
                cm.Parameters.AddWithValue("@TicketStatus", TicketStatus.Open.ToString());
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    TicketHistoryInfo info = TicketHistoryInfo.GetTicketHistoryInfo(dr);
                    Add(info);
                }
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch(CriteriaCloseTicketByUser criteria)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = @"SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, 
                                    CreatedDate, UpdatedBy, UpdatedDate, UpdateCommand FROM ticket_his 
                                    WHERE Escalation=@Escalation AND TicketStatus=@TicketStatus";
                cm.Parameters.AddWithValue("@Escalation", criteria.UserName);
                cm.Parameters.AddWithValue("@TicketStatus", TicketStatus.Closed.ToString());
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    TicketHistoryInfo info = TicketHistoryInfo.GetTicketHistoryInfo(dr);
                    Add(info);
                }
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch(string username)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = @"SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, 
                                    CreatedDate, UpdatedBy, UpdatedDate, UpdateCommand FROM ticket_his 
                                    WHERE Escalation=@Escalation";
                cm.Parameters.AddWithValue("@Escalation", username);
                //cm.Parameters.AddWithValue("@TicketStatus", TicketStatus.Open.ToString());
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketHistoryInfo info = TicketHistoryInfo.GetTicketHistoryInfo(dr);
                    Add(info);
                }
                cn.Close();
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch(CriteriaOpenTicket criteria)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = @"SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, 
                                    CreatedDate, UpdatedBy, UpdatedDate, UpdateCommand FROM ticket_his 
                                    WHERE TicketStatus=@TicketStatus";
                cm.Parameters.AddWithValue("@TicketStatus", TicketStatus.Open.ToString());
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketHistoryInfo info = TicketHistoryInfo.GetTicketHistoryInfo(dr);
                    Add(info);
                }
                cn.Close();
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch(CriteriaActivity criteria)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = @"SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, 
                                    CreatedDate, UpdatedBy, UpdatedDate, UpdateCommand FROM ticket_his 
                                    WHERE Requester=@Requester";
                cm.Parameters.AddWithValue("@Requester", criteria.Email);
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketHistoryInfo info = TicketHistoryInfo.GetTicketHistoryInfo(dr);
                    Add(info);
                }
                cn.Close();
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch(CriteriaActivityTicket criteria)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = @"SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, 
                                    CreatedDate, UpdatedBy, UpdatedDate, UpdateCommand FROM ticket_his 
                                    WHERE TicketNo=@TicketNo";
                cm.Parameters.AddWithValue("@TicketNo", criteria.TIcketNo);
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketHistoryInfo info = TicketHistoryInfo.GetTicketHistoryInfo(dr);
                    Add(info);
                }
                cn.Close();
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch(CriteriaCloseTicket criteria)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = @"SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, 
                                    CreatedDate, UpdatedBy, UpdatedDate, UpdateCommand FROM ticket_his 
                                    WHERE TicketStatus=@TicketStatus";
                cm.Parameters.AddWithValue("@TicketStatus", TicketStatus.Closed.ToString());
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketHistoryInfo info = TicketHistoryInfo.GetTicketHistoryInfo(dr);
                    Add(info);
                }
                cn.Close();
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch(CriteriaByDate criteria)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = @"SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, 
                                    CreatedDate, UpdatedBy, UpdatedDate, UpdateCommand FROM ticket_his 
                                    WHERE CreatedDate >= @startDate AND CreatedDate <= @endDate";
                cm.Parameters.AddWithValue("@startDate", criteria.StartDate);
                cm.Parameters.AddWithValue("@endDate", criteria.EndDate);
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketHistoryInfo info = TicketHistoryInfo.GetTicketHistoryInfo(dr);
                    Add(info);
                }
                cn.Close();
            }
            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch()
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = "SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate, UpdateCommand FROM ticket_his";
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketHistoryInfo info = TicketHistoryInfo.GetTicketHistoryInfo(dr);
                    Add(info);
                }
                cn.Close();
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch(CriteriaActivityTicketByNoTiketAndUser criteria)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = @"SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, 
                                    CreatedDate, UpdatedBy, UpdatedDate, UpdateCommand FROM ticket_his 
                                    WHERE TicketNo=@TicketNo and  Escalation=@Escalation order by UpdatedDate desc";
                cm.Parameters.AddWithValue("@TicketNo", criteria.TicketNo);
                cm.Parameters.AddWithValue("@Escalation", criteria.UserName);
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketHistoryInfo info = TicketHistoryInfo.GetTicketHistoryInfo(dr);
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
