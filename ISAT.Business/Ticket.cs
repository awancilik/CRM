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
    public class Ticket : BusinessBase<Ticket>
    {

        #region Property

        public static readonly PropertyInfo<long> idticketProperty = RegisterProperty<long>(c => c.idticket);
        public long idticket
        {
            get { return GetProperty(idticketProperty); }
            private set { SetProperty(idticketProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketNoProperty = RegisterProperty<string>(c => c.TicketNo);
        [Required]
        public string TicketNo
        {
            get { return GetProperty(TicketNoProperty); }
            set { SetProperty(TicketNoProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketSubjectProperty = RegisterProperty<string>(c => c.TicketSubject);
        [Required]
        public string TicketSubject
        {
            get { return GetProperty(TicketSubjectProperty); }
            set { SetProperty(TicketSubjectProperty, value); }
        }


        public static readonly PropertyInfo<string> RequesterProperty = RegisterProperty<string>(c => c.Requester);
        [Required]
        public string Requester
        {
            get { return GetProperty(RequesterProperty); }
            set { SetProperty(RequesterProperty, value); }
        }


        public static readonly PropertyInfo<string> CallerPosistionProperty = RegisterProperty<string>(c => c.CallerPosistion); 
        public string CallerPosistion
        {
            get { return GetProperty(CallerPosistionProperty); }
            set { SetProperty(CallerPosistionProperty, value); }
        }


        public static readonly PropertyInfo<string> PriorityProperty = RegisterProperty<string>(c => c.Priority);
        [Required]
        public string Priority
        {
            get { return GetProperty(PriorityProperty); }
            set { SetProperty(PriorityProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketOwnerProperty = RegisterProperty<string>(c => c.TicketOwner);
        [Required]
        public string TicketOwner
        {
            get { return GetProperty(TicketOwnerProperty); }
            set { SetProperty(TicketOwnerProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketTypeProperty = RegisterProperty<string>(c => c.TicketType);
        [Required]
        public string TicketType
        {
            get { return GetProperty(TicketTypeProperty); }
            set { SetProperty(TicketTypeProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketStatusProperty = RegisterProperty<string>(c => c.TicketStatus);
        [Required]
        public string TicketStatus
        {
            get { return GetProperty(TicketStatusProperty); }
            set { SetProperty(TicketStatusProperty, value); }
        }


        public static readonly PropertyInfo<string> TicketDescriptionProperty = RegisterProperty<string>(c => c.TicketDescription);
        public string TicketDescription
        {
            get { return GetProperty(TicketDescriptionProperty); }
            set { SetProperty(TicketDescriptionProperty, value); }
        }


        public static readonly PropertyInfo<string> SolutionProperty = RegisterProperty<string>(c => c.Solution);
        [Required]
        public string Solution
        {
            get { return GetProperty(SolutionProperty); }
            set { SetProperty(SolutionProperty, value); }
        }


        public static readonly PropertyInfo<string> EscalationProperty = RegisterProperty<string>(c => c.Escalation);
        [Required]
        public string Escalation
        {
            get { return GetProperty(EscalationProperty); }
            set { SetProperty(EscalationProperty, value); }
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
        
        #endregion

        #region Factory Method

        public static Ticket NewTicket()
        {
            Ticket obj =  DataPortal.Create<Ticket>();
            return obj;
        }

        public static Ticket GetTicket(long idticket)
        {
            Ticket obj = DataPortal.Fetch<Ticket>(idticket);
            if (obj.idticket == 0)
            {
                return null;
            }
            return obj;
        }

        public static Ticket GetTicket(string ticketNo)
        {
            Ticket obj = DataPortal.Fetch<Ticket>(ticketNo);
            if (obj.idticket == 0)
            {
                return null;
            }
            return obj;
        }

        public static Ticket GetTicket(SafeDataReader dr)
        {
            return DataPortal.Fetch<Ticket>(dr);
        }

        public static void DeleteTicket(long idticket)
        {
            DataPortal.Delete<Ticket>(idticket);
        }

        private Ticket()
        {

        }

        #endregion

        #region Data Access

        protected override void DataPortal_Create()
        {
            base.DataPortal_Create(); 
            using (BypassPropertyChecks)
            {

            }
        }

        private void DataPortal_Fetch(long id)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM ticket WHERE idticket=@idticket";
                cm.Parameters.AddWithValue("@idticket", id);
                SafeDataReader dr = ctx.Read(cm);
                while (dr.Read())
                {
                    DataPortal_Fetch(dr);
                }
            }
        }

        private void DataPortal_Fetch(string ticketNo)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateSelectCommand();
                cm.CommandText = "SELECT idticket, TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate FROM ticket WHERE TicketNo=@TicketNo";
                cm.Parameters.AddWithValue("@TicketNo", ticketNo);
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
                cm.CommandText = "INSERT INTO ticket(TicketNo, TicketSubject, Requester, CallerPosistion, Priority, TicketOwner, TicketType, TicketStatus, TicketDescription, Solution, Escalation, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate) VALUES (@TicketNo, @TicketSubject, @Requester, @CallerPosistion, @Priority, @TicketOwner, @TicketType, @TicketStatus, @TicketDescription, @Solution, @Escalation, @CreatedBy, @CreatedDate, @UpdatedBy, @UpdatedDate)";
                DoCommand(cm);
                cm.CommandText = "SELECT LAST_INSERT_ID()";
                SetProperty(idticketProperty, Convert.ToInt64(cm.ExecuteScalar()));
            }
        }

        protected override void DataPortal_Update()
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateCommand();
                cm.CommandText = "UPDATE ticket SET TicketNo=@TicketNo, TicketSubject=@TicketSubject, Requester=@Requester, CallerPosistion=@CallerPosistion, Priority=@Priority, TicketOwner=@TicketOwner, TicketType=@TicketType, TicketStatus=@TicketStatus, TicketDescription=@TicketDescription, Solution=@Solution, Escalation=@Escalation, CreatedBy=@CreatedBy, CreatedDate=@CreatedDate, UpdatedBy=@UpdatedBy, UpdatedDate=@UpdatedDate WHERE idticket=@idticket";
                cm.Parameters.AddWithValue("@idticket", idticket);
                DoCommand(cm);
            }
        }

        protected override void DataPortal_DeleteSelf()
        {
            DataPortal_Delete(ReadProperty(idticketProperty));
        }

        private void DataPortal_Delete(long id)
        {
            using (DatabaseManager ctx = new DatabaseManager())
            {
                MySqlCommand cm = ctx.CreateCommand();
                cm.CommandText = "DELETE FROM ticket WHERE idticket=@idticket";
                cm.Parameters.AddWithValue("@idticket", idticket);
                cm.ExecuteNonQuery();
            }
        }

        private void DataPortal_Fetch(SafeDataReader dr)
        {
            SetProperty(idticketProperty, dr.GetInt64(0));
            SetProperty(TicketNoProperty, dr.GetString(1));
            SetProperty(TicketSubjectProperty, dr.GetString(2));
            SetProperty(RequesterProperty, dr.GetString(3));
            SetProperty(CallerPosistionProperty, dr.GetString(4));
            SetProperty(PriorityProperty, dr.GetString(5));
            SetProperty(TicketOwnerProperty, dr.GetString(6));
            SetProperty(TicketTypeProperty, dr.GetString(7));
            SetProperty(TicketStatusProperty, dr.GetString(8));
            SetProperty(TicketDescriptionProperty, dr.GetString(9));
            SetProperty(SolutionProperty, dr.GetString(10));
            SetProperty(EscalationProperty, dr.GetString(11));
            SetProperty(CreatedByProperty, dr.GetString(12));
            SetProperty(CreatedDateProperty, dr.GetDateTime(13));
            SetProperty(UpdatedByProperty, dr.GetString(14));
            SetProperty(UpdatedDateProperty, dr.GetDateTime(15));            
        }

        private int DoCommand(MySqlCommand cm)
        {
            int rowAffected = 0;

            cm.Parameters.AddWithValue("@TicketNo", TicketNo);
            cm.Parameters.AddWithValue("@TicketSubject", TicketSubject);
            cm.Parameters.AddWithValue("@Requester", Requester);
            cm.Parameters.AddWithValue("@CallerPosistion", CallerPosistion);
            cm.Parameters.AddWithValue("@Priority", Priority);
            cm.Parameters.AddWithValue("@TicketOwner", TicketOwner);
            cm.Parameters.AddWithValue("@TicketType", TicketType);
            cm.Parameters.AddWithValue("@TicketStatus", TicketStatus);
            cm.Parameters.AddWithValue("@TicketDescription", TicketDescription);
            cm.Parameters.AddWithValue("@Solution", Solution);
            cm.Parameters.AddWithValue("@Escalation", Escalation);
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
