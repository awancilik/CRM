using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ISAT.Web.ViewModel
{
    public class Customer
    {
        public long IdCustomer { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"(\()?(\+62|62|0)(\d{2,3})?\)?[ .-]?\d{2,4}[ .-]?\d{2,4}[ .-]?\d{2,4}", ErrorMessage = "Not a valid Phone number")]
        public string Custno { get; set; }

        [Required(ErrorMessage = "Input is required.")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; } 
    }

    public class CustomerProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Custno { get; set; }
        public long TicketId { get; set; }
        public string Subject { get; set; }
        public string Requester { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string Priority { get; set; }
        public string TicketOwner { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
    }
}