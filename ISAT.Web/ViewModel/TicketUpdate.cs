using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISAT.Web.ViewModel
{
    public class TicketUpdate
    {
        public string TicketID { get; set; }
        public string Status { get; set; }
        public string Owner { get; set; }
        public string UpdatedDate { get; set; }
    }
}