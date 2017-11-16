using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APILA.CRM
{
    public class CRMCall
    {
        public string _event { get; set; }
        public string callId { get; set; }
        public string incoming { get; set; }
        public string type { get; set; }
        public string acdId { get; set; }
        public string displayNumber { get; set; }
        public string displayLabel { get; set; }
        public string displayNumberE164 { get; set; }
        public DateTime AddedOn { get; set; }
        public string terminalId { get; set; }
        public string userCrm { get; set; }
    }
}
