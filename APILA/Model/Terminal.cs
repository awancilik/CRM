using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APILA.Model
{
    public class Terminal
    {
        public int uiPos { get; set; }
        public int maxTerminalConnection { get; set; }
        public int ringDelayed { get; set; }
        public string label { get; set; }
        public string type { get; set; }
        public PhoneNumber phoneNumber { get; set; }
        public string physicalState { get; set; }
        public int timePhysicalStateOutOfService { get; set; }
        public int abstractTerminalID { get; set; }
        public string alias { get; set; }
        public Device device { get; set; }
        
    }
}
