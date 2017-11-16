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
    public class TicketHistoryInfo : ReadOnlyBase<TicketHistoryInfo>
    {

        #region Property


        public static readonly PropertyInfo<long> idticketProperty = RegisterProperty<long>(c => c.idticket);
        public long idticket
        {
            get { return GetProperty(idticketProperty); }
            private set { LoadProperty(idticketProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketNoProperty = RegisterProperty<string>(c => c.TicketNo);
        public string TicketNo
        {
            get { return GetProperty(TicketNoProperty); }
            private set { LoadProperty(TicketNoProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketSubjectProperty = RegisterProperty<string>(c => c.TicketSubject);
        public string TicketSubject
        {
            get { return GetProperty(TicketSubjectProperty); }
            private set { LoadProperty(TicketSubjectProperty, value); }
        }


        public static readonly PropertyInfo<string> RequesterProperty = RegisterProperty<string>(c => c.Requester);
        public string Requester
        {
            get { return GetProperty(RequesterProperty); }
            private set { LoadProperty(RequesterProperty, value); }
        }


        public static readonly PropertyInfo<string> CallerPosistionProperty = RegisterProperty<string>(c => c.CallerPosistion);
        public string CallerPosistion
        {
            get { return GetProperty(CallerPosistionProperty); }
            private set { LoadProperty(CallerPosistionProperty, value); }
        }


        public static readonly PropertyInfo<string> PriorityProperty = RegisterProperty<string>(c => c.Priority);
        public string Priority
        {
            get { return GetProperty(PriorityProperty); }
            private set { LoadProperty(PriorityProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketOwnerProperty = RegisterProperty<string>(c => c.TicketOwner);
        public string TicketOwner
        {
            get { return GetProperty(TicketOwnerProperty); }
            private set { LoadProperty(TicketOwnerProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketTypeProperty = RegisterProperty<string>(c => c.TicketType);
        public string TicketType
        {
            get { return GetProperty(TicketTypeProperty); }
            private set { LoadProperty(TicketTypeProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketStatusProperty = RegisterProperty<string>(c => c.TicketStatus);
        public string TicketStatus
        {
            get { return GetProperty(TicketStatusProperty); }
            private set { LoadProperty(TicketStatusProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketDescriptionProperty = RegisterProperty<string>(c => c.TicketDescription);
        public string TicketDescription
        {
            get { return GetProperty(TicketDescriptionProperty); }
            private set { LoadProperty(TicketDescriptionProperty, value); }
        }


        public static readonly PropertyInfo<string> SolutionProperty = RegisterProperty<string>(c => c.Solution);
        public string Solution
        {
            get { return GetProperty(SolutionProperty); }
            private set { LoadProperty(SolutionProperty, value); }
        }


        public static readonly PropertyInfo<string> EscalationProperty = RegisterProperty<string>(c => c.Escalation);
        public string Escalation
        {
            get { return GetProperty(EscalationProperty); }
            private set { LoadProperty(EscalationProperty, value); }
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

        public static readonly PropertyInfo<string> UpdateCommandProperty = RegisterProperty<string>(c => c.UpdateCommand);
        public string UpdateCommand
        {
            get { return GetPropertyConvert<string, string>(UpdateCommandProperty); }
            private set { LoadPropertyConvert<string, string>(UpdateCommandProperty, value); }
        }

        #endregion


        #region Factory Method

        public static List<string> GetPriority()
        {
            List<string> list = new List<string>();
            list.Add(ISAT.Business.Priority.Urgent.ToString());
            list.Add(ISAT.Business.Priority.High.ToString());
            list.Add(ISAT.Business.Priority.Medium.ToString());
            list.Add(ISAT.Business.Priority.Low.ToString());

            return list;
        }

        public static TicketHistoryInfo GetTicketHistoryInfo(long idticket)
        {
            return DataPortal.Fetch<TicketHistoryInfo>(idticket);
        }

        public static TicketHistoryInfo GetTicketHistoryInfo(SafeDataReader dr)
        {
            return DataPortal.Fetch<TicketHistoryInfo>(dr);
        }

        private TicketHistoryInfo()
        {

        }
        #endregion



        #region Data Access

        private void DataPortal_Fetch(long id)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate, UpdateCommand FROM ticket_his WHERE idticket=@idticket";
                cm.Parameters.AddWithValue("@idticket", id);
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    DataPortal_Fetch(dr);
                }
            }
        }

        private void DataPortal_Fetch(SafeDataReader dr)
        {
            LoadProperty(idticketProperty, dr.GetInt64(0));
            LoadProperty(TicketNoProperty, dr.GetString(1));
            LoadProperty(TicketSubjectProperty, dr.GetString(2));
            LoadProperty(RequesterProperty, dr.GetString(3));
            LoadProperty(CallerPosistionProperty, dr.GetString(4));
            LoadProperty(PriorityProperty, dr.GetString(5));
            LoadProperty(TicketOwnerProperty, dr.GetString(6));
            LoadProperty(TicketTypeProperty, dr.GetString(7));
            LoadProperty(TicketStatusProperty, dr.GetString(8));
            LoadProperty(TicketDescriptionProperty, dr.GetString(9));
            LoadProperty(SolutionProperty, dr.GetString(10));
            LoadProperty(EscalationProperty, dr.GetString(11));
            LoadProperty(CreatedByProperty, dr.GetString(12));
            LoadProperty(CreatedDateProperty, dr.GetDateTime(13));
            LoadProperty(UpdatedByProperty, dr.GetString(14));
            LoadProperty(UpdatedDateProperty, dr.GetDateTime(15));
            LoadProperty(UpdateCommandProperty, dr.GetString(16));
        }

        #endregion

    }
}
