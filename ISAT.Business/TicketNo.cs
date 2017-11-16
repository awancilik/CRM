using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Csla;
using Csla.Serialization;


namespace ISAT.Business
{
    [Serializable]  
    public class TicketNo : CommandBase<TicketNo>
    {
        public static readonly PropertyInfo<string> AgentIdProperty = RegisterProperty<string>(c => c.AgentId);
        public string AgentId
        {
            get { return ReadProperty(AgentIdProperty); }
            private set { LoadProperty(AgentIdProperty, value); }
        }

        public static readonly PropertyInfo<string> TableNameProperty = RegisterProperty<string>(c => c.TableName);
        public string TableName
        {
            get { return ReadProperty(TableNameProperty); }
            private set { LoadProperty(TableNameProperty, value); }
        }

        public static readonly PropertyInfo<string> NumberProperty = RegisterProperty<string>(c => c.Number);
        public string Number
        {
            get { return ReadProperty(NumberProperty); }
            private set { LoadProperty(NumberProperty, value); }
        }

        public static string Create(string agentId, string tableName)
        {
            TicketNo cmd = new TicketNo { AgentId = agentId, TableName=tableName };
            cmd = DataPortal.Execute<TicketNo>(cmd);
            return cmd.Number;
        }

        protected override void DataPortal_Execute()
        {
            SequenceNumber seq = SequenceNumber.GetSequenceNumber(TableName, AgentId);
            if (seq==null)
            {
                seq = SequenceNumber.NewSequenceNumber();
                seq.TableName = TableName;
                seq.Prefix = AgentId;
                seq.SeqDate = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd"));
                seq.SeqNumber = 1;
                seq = seq.Save();
            }
            else
            {
                SequenceNumber newsequence = SequenceNumber.NewSequenceNumber();
                newsequence.TableName = seq.TableName;
                newsequence.Prefix = seq.Prefix;
                newsequence.SeqDate = seq.SeqDate;
                newsequence.SeqNumber = seq.SeqNumber + 1;
                newsequence = newsequence.Save();
                seq = newsequence;
            }

            Number = seq.Prefix + seq.SeqDate.ToString() +  seq.SeqNumber.ToString().PadLeft(4,'0'); 
        }

    }
}
