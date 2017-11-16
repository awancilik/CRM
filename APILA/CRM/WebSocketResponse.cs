using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APILA.CRM
{
    public class WebSocketResponse
    {
        public int maxInactiveInterval_seconds { get; set; }
        public string id { get; set; }
        public string timeout_milliseconds { get; set; }
        public string address { get; set; }
        public string maxMessageSize { get; set; }
        public int maxFrameLength { get; set; }
    }
}
