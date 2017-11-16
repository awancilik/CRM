using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISAT.Web.ViewModel
{
    public class CustomerCriteria
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string CustNo { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public DateTime? CreatedDate { get; set; } = DateTime.Today;

        public string CreatedBy { get; set; } = string.Empty;
    }
}