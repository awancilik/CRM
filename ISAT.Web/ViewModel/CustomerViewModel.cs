using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISAT.Web.ViewModel
{
    public class CustomerViewModel
    {
        public List<Business.Customer> ListCustomer { get; set; } = new List<Business.Customer>();

        public CustomerCriteria Criteria { get; set; }
    }
}