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
    public class TicketInfoList : ReadOnlyBindingListBase<TicketInfoList, TicketInfo>
    {

        #region Factory Method

        public static TicketInfoList GetTicketInfoList()
        {
            return DataPortal.Fetch<TicketInfoList>();
        }

        public static TicketInfoList GetTicketInbox(string username)
        {
            return DataPortal.Fetch<TicketInfoList>(username);
        }

        public static TicketInfoList GetReportOpenTicket()
        {
            return DataPortal.Fetch<TicketInfoList>(new CriteriaOpenTicket());
        }

        public static TicketInfoList GetReportOpenTicketCurrentUser(string username)
        {
            return DataPortal.Fetch<TicketInfoList>(new CriteriaOpenTicketByUser() { UserName = username });
        }

        public static TicketInfoList GetReportByDate(DateTime startDate, DateTime endDate)
        {
            return DataPortal.Fetch<TicketInfoList>(new CriteriaByDate() { StartDate = startDate, EndDate = endDate });
        }

        public static TicketInfoList GetReportByDate(DateTime startDate, DateTime endDate, string ticketSubject)
        {
            return DataPortal.Fetch<TicketInfoList>(new CriteriaByDate() { StartDate = startDate, EndDate = endDate });
        }

        public static TicketInfoList GetReportByDateAndTicketType(DateTime startDate, DateTime endDate, string ticketTypeId)
        {
            return DataPortal.Fetch<TicketInfoList>(new CriteriaByDateAndTicketType() { StartDate = startDate, EndDate = endDate, TicketType = ticketTypeId });
        }

        public static TicketInfoList GetReportCloseTicket()
        {
            return DataPortal.Fetch<TicketInfoList>(new CriteriaCloseTicket());
        }

        public static TicketInfoList GetReportCloseTicketCurrentUser(string username)
        {
            return DataPortal.Fetch<TicketInfoList>(new CriteriaCloseTicketByUser() { UserName = username });
        }

        public static TicketInfoList GetOpenTicketByRequester(string requester)
        {
            return DataPortal.Fetch<TicketInfoList>(new CriteriaOpenTicketByRequeser() { Requester = requester });
        }

        public static TicketInfoList GetClosedTicketByRequester(string requester)
        {
            return DataPortal.Fetch<TicketInfoList>(new CriteriaClosedTicketByRequeser() { Requester = requester });
        }

        private TicketInfoList()
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
        private class CriteriaByDateAndTicketSubject : CriteriaBase<CriteriaByDateAndTicketSubject>
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string TicketSubject { get; set; }
        }

        [Serializable()]
        private class CriteriaByDateAndTicketType : CriteriaBase<CriteriaByDate>
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string TicketType { get; set; }
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
        private class CriteriaOpenTicketByRequeser : CriteriaBase<CriteriaOpenTicketByRequeser>
        {
            public string Requester { get; set; }
        }

        [Serializable()]
        private class CriteriaClosedTicketByRequeser : CriteriaBase<CriteriaClosedTicketByRequeser>
        {
            public string Requester { get; set; }
        }

        private void DataPortal_Fetch(CriteriaOpenTicketByUser criteria)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = @"SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, 
                                    CreatedDate, UpdatedBy, UpdatedDate FROM ticket 
                                    WHERE Escalation=@Escalation AND TicketStatus=@TicketStatus";
                cm.Parameters.AddWithValue("@Escalation", criteria.UserName);
                cm.Parameters.AddWithValue("@TicketStatus", TicketStatus.Open.ToString());
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketInfo info = TicketInfo.GetTicketInfo(dr);
                    Add(info);
                }
                cn.Close();
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch(CriteriaCloseTicketByUser criteria)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = @"SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, 
                                    CreatedDate, UpdatedBy, UpdatedDate FROM ticket 
                                    WHERE Escalation=@Escalation AND TicketStatus=@TicketStatus";
                cm.Parameters.AddWithValue("@Escalation", criteria.UserName);
                cm.Parameters.AddWithValue("@TicketStatus", TicketStatus.Closed.ToString());
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketInfo info = TicketInfo.GetTicketInfo(dr);
                    Add(info);
                }
                cn.Close();
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
                                    CreatedDate, UpdatedBy, UpdatedDate, '' FROM ticket 
                                    WHERE Escalation=@Escalation AND TicketStatus=@TicketStatus";
                cm.Parameters.AddWithValue("@Escalation", username);
                cm.Parameters.AddWithValue("@TicketStatus", TicketStatus.Open.ToString());
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketInfo info = TicketInfo.GetTicketInfo(dr);
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
                cm.CommandText = @"SELECT t.idticket, t.TicketNo, t.TicketSubject, t.Requester, t.CallerPosistion, t.Priority, t.TicketOwner, t.TicketType, t.TicketStatus, t.TicketDescription, 
                                    t.Solution, t.Escalation, t.CreatedBy, t.CreatedDate, t.UpdatedBy, t.UpdatedDate, CONCAT_WS(' ', c.FirstName, c.LastName) AS CustomerName FROM ticket t
                                    JOIN customer c ON t.requester =  c.email 
                                    WHERE TicketStatus=@TicketStatus";
                cm.Parameters.AddWithValue("@TicketStatus", TicketStatus.Open.ToString());
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketInfo info = TicketInfo.GetTicketInfo(dr);
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
                cm.CommandText = @"SELECT t.idticket, t.TicketNo, t.TicketSubject, t.Requester, t.CallerPosistion, t.Priority, t.TicketOwner, t.TicketType, t.TicketStatus, t.TicketDescription, 
                                    t.Solution, t.Escalation, t.CreatedBy, t.CreatedDate, t.UpdatedBy, t.UpdatedDate, CONCAT_WS(' ', c.FirstName, c.LastName) AS CustomerName FROM ticket t
                                    JOIN customer c ON t.requester =  c.email 
                                    WHERE t.TicketStatus=@TicketStatus";
                cm.Parameters.AddWithValue("@TicketStatus", TicketStatus.Closed.ToString());
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketInfo info = TicketInfo.GetTicketInfo(dr);
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
                                    CreatedDate, UpdatedBy, UpdatedDate FROM ticket 
                                    WHERE CreatedDate >= @startDate AND CreatedDate <= @endDate ";
                cm.Parameters.AddWithValue("@startDate", criteria.StartDate);
                cm.Parameters.AddWithValue("@endDate", criteria.EndDate);
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketInfo info = TicketInfo.GetTicketInfo(dr);
                    Add(info);
                }
                cn.Close();
            }
            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch(CriteriaByDateAndTicketSubject criteria)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = @"SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, 
                                    CreatedDate, UpdatedBy, UpdatedDate FROM ticket 
                                    WHERE CreatedDate >= @startDate AND CreatedDate <= @endDate AND TicketSubject in (SELECT tickettypename FROM tickettype where ticketcategory = @ticketsubject)";
                cm.Parameters.AddWithValue("@startDate", criteria.StartDate);
                cm.Parameters.AddWithValue("@endDate", criteria.EndDate);
                cm.Parameters.AddWithValue("@ticketsubject", criteria.TicketSubject);
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketInfo info = TicketInfo.GetTicketInfo(dr);
                    Add(info);
                }
                cn.Close();
            }
            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch(CriteriaByDateAndTicketType criteria)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = @"SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, 
                                    CreatedDate, UpdatedBy, UpdatedDate FROM ticket 
                                    WHERE CreatedDate >= @startDate AND CreatedDate <= @endDate and TicketType = @TicketType";
                cm.Parameters.AddWithValue("@startDate", criteria.StartDate);
                cm.Parameters.AddWithValue("@endDate", criteria.EndDate);
                cm.Parameters.AddWithValue("@TicketType", criteria.TicketType);
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketInfo info = TicketInfo.GetTicketInfo(dr);
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
                cm.CommandText = "SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM ticket";
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketInfo info = TicketInfo.GetTicketInfo(dr);
                    Add(info);
                }
                cn.Close();
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch(CriteriaOpenTicketByRequeser criteria)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = @"SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, 
                                    CreatedDate, UpdatedBy, UpdatedDate FROM ticket 
                                    WHERE Requester=@Requester AND TicketStatus=@TicketStatus";
                cm.Parameters.AddWithValue("@Requester", criteria.Requester);
                cm.Parameters.AddWithValue("@TicketStatus", TicketStatus.Open.ToString());
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketInfo info = TicketInfo.GetTicketInfo(dr);
                    Add(info);
                }
                cn.Close();
            }

            IsReadOnly = true;
            RaiseListChangedEvents = rlce;
        }

        private void DataPortal_Fetch(CriteriaClosedTicketByRequeser criteria)
        {
            var rlce = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            IsReadOnly = false;

            using (MySqlConnection cn = new MySqlConnection(Database.ConnectionString))
            {
                cn.Open();
                MySqlCommand cm = cn.CreateCommand();
                cm.CommandText = @"SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, 
                                    CreatedDate, UpdatedBy, UpdatedDate FROM ticket 
                                    WHERE Requester=@Requester AND TicketStatus=@TicketStatus";
                cm.Parameters.AddWithValue("@Requester", criteria.Requester);
                cm.Parameters.AddWithValue("@TicketStatus", TicketStatus.Closed.ToString());
                SafeDataReader dr = new SafeDataReader(cm.ExecuteReader());
                while (dr.Read())
                {
                    TicketInfo info = TicketInfo.GetTicketInfo(dr);
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
