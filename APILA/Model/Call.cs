using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APILA.Model
{
    public class Call
    {
        public int callId { get; set; }
        public bool incoming { get; set; }
        public string type { get; set; }
        public int displayNumber { get; set; }
        public string displayLabel { get; set; }
        public string displayNumberE164 { get; set; }
    }
}
