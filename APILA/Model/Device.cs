using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APILA.Model
{
    public class Device
    {
        public string dynamicInternalIpAddress { get; set; }
        public Site site { get; set; }
        public string deviceModelLabel { get; set; }
        public bool isMacAddressNeeded { get; set; }
        public string alias { get; set; }
        public string userAgent { get; set; }
        public string label { get; set; }
        public string deviceID { get; set; }
    }
}
