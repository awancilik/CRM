using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISAT.Business;
using ISAT.Web.ViewModel;
using X.PagedList;
using ISAT.Web.Helpers;

namespace ISAT.Web.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private int pageSize = 10;

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexCustomer(CustomerCriteria criteria, string currentFilter, int? page = 1)
        {
            int pageNumber = (page ?? 1);
            this.ViewBag.CurrentFilter = criteria;
            if (!string.IsNullOrEmpty(currentFilter))
            {
                return this.IndexCustomer(criteria, page);
            }
            var obj = CustomerList.GetCustomerList().OrderByDescending(x => x.CreatedDate).ToPagedList(pageNumber, this.pageSize);
            var list = new ISAT.Web.ViewModel.CustomerViewModel();
            foreach (var item in obj)
            {
                var cust = Business.Customer.NewCustomer();
                cust.FirstName = item.FirstName;
                cust.LastName = item.LastName;
                cust.Email = item.Email;
                cust.CustNo = item.CustNo;
                cust.CompanyName = item.CompanyName;
                cust.CreatedDate = item.CreatedDate;
                cust.CreatedBy = item.CreatedBy;
                list.ListCustomer.Add(cust);
            }

            var viewModel = new CustomerViewModel()
            {
                ListCustomer = list.ListCustomer.ToList(),
                Criteria = new CustomerCriteria()
            };
            var onePageOfProducts = list;
            ViewBag.OnePageOfProducts = obj;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult IndexCustomer(CustomerCriteria criteria, int? page = 1)
        {
            int pageNumber = (page ?? 1);
            this.ViewBag.CurrentFilter = criteria;

            var obj = CustomerList.GetCustomerList().Where(o => (criteria.FirstName == null || o.FirstName.ToLower().Contains(criteria.FirstName.ToLower())) &&
                                            (criteria.LastName == null || o.LastName.ToLower().Contains(criteria.LastName.ToLower())) &&
                                            (criteria.Email == null || o.Email.ToLower().Contains(criteria.Email.ToLower())) &&
                                            (criteria.CompanyName == null || o.CompanyName.ToLower().Contains(criteria.CompanyName.ToLower()) &&
                                            (criteria.CustNo == null || o.CustNo.ToLower().Contains(criteria.CustNo.ToLower()))))
                                            .Select(o => o).ToPagedList(pageNumber, this.pageSize);
            var list = new ISAT.Web.ViewModel.CustomerViewModel();
            foreach (var item in obj)
            {
                var cust = Business.Customer.NewCustomer();
                cust.FirstName = item.FirstName;
                cust.LastName = item.LastName;
                cust.Email = item.Email;
                cust.CustNo = item.CustNo;
                cust.CompanyName = item.CompanyName;
                cust.CreatedDate = item.CreatedDate;
                cust.CreatedBy = item.CreatedBy;
                list.ListCustomer.Add(cust);
            }
            var viewModel = new CustomerViewModel()
            {
                ListCustomer = list.ListCustomer.OrderByDescending(x => x.CreatedDate).ToList(),
                Criteria = new CustomerCriteria()
            };
            var onePageOfProducts = list;
            ViewBag.OnePageOfProducts = obj;
            return View(viewModel);
        }

        public ActionResult CustomerProfile(string email)
        {
            List<ISAT.Web.ViewModel.CustomerProfile> list = new List<ViewModel.CustomerProfile>();
            if (!string.IsNullOrEmpty(email))
            {
                Business.Customer cust = Business.Customer.GetCustomer(email);
                TicketInfo ticket = TicketInfo.GetTicketInfo(cust.Email);
                if (cust != null)
                {
                    if (ticket != null)
                    {
                        var custList = Business.CustomerInfoList.GetCustomerInfoList().Where(x => x.Email == ticket.Requester);
                        var openList = ISAT.Business.TicketInfoList.GetOpenTicketByRequester(ticket.Requester).OrderByDescending(x => x.UpdatedDate);
                        var closedList = ISAT.Business.TicketInfoList.GetClosedTicketByRequester(ticket.Requester).OrderByDescending(x => x.UpdatedDate);

                        var newopenList = from c in openList
                                          select new CustomerProfile
                                          {
                                              TicketId = c.idticket,
                                              Subject = c.TicketSubject,
                                              Requester = GetRequesterFullName(c.Requester),
                                              CreatedDate = c.CreatedDate.ToString("dd MMM yyyy HH':'mm"),
                                              UpdatedDate = c.UpdatedDate.ToString("dd MMM yyyy HH':'mm"),
                                              Priority = c.Priority,
                                              TicketOwner = c.TicketOwner,
                                              Status = c.TicketStatus
                                          };

                        var newcloseList = from c in closedList
                                           select new CustomerProfile
                                           {
                                               TicketId = c.idticket,
                                               Subject = c.TicketSubject,
                                               Requester = GetRequesterFullName(c.Requester),
                                               CreatedDate = c.CreatedDate.ToString("dd MMM yyyy HH':'mm"),
                                               UpdatedDate = c.UpdatedDate.ToString("dd MMM yyyy HH':'mm"),
                                               Priority = c.Priority,
                                               TicketOwner = c.TicketOwner,
                                               Status = c.TicketStatus,
                                               CreatedBy = c.CreatedBy
                                           };

                        var newcustomerList = from c in custList
                                              select new CustomerProfile
                                              {
                                                  FirstName = c.FirstName,
                                                  LastName = c.LastName,
                                                  Custno = c.CustNo,
                                                  Email = c.Email,
                                                  CompanyName = c.CompanyName,
                                              };

                        list.AddRange(newcustomerList);
                        list.AddRange(newopenList);
                        list.AddRange(newcloseList);
                    }
                    return View(list);
                }
                else
                {
                    ViewBag.Message = " Detail Not Found.";
                    return RedirectToAction("IndexCustomer");
                }
            }

            return View();
        }

        private string GetRequesterFullName(string emailid)
        {
            CustomerInfo info = CustomerInfo.GetCustomerInfo(emailid);
            if (info != null)
            {
                return info.FirstName + " " + info.LastName;
            }
            return string.Empty;
        }

        public ActionResult ExportExcelCustomerData(CustomerCriteria criteria)
        {
            var headerDictionary = new Dictionary<string, string>();

            this.ViewBag.CurrentFilter = criteria;

            var obj = CustomerList.GetCustomerList();
            var list = new ISAT.Web.ViewModel.CustomerViewModel();
            foreach (var item in obj)
            {
                var cust = Business.Customer.NewCustomer();
                cust.FirstName = item.FirstName;
                cust.LastName = item.LastName;
                cust.Email = item.Email;
                cust.CustNo = item.CustNo;
                cust.CompanyName = item.CompanyName;
                cust.CreatedDate = item.CreatedDate;
                cust.CreatedBy = item.CreatedBy;
                list.ListCustomer.Add(cust);
            }

            var viewModel = new CustomerViewModel()
            {
                ListCustomer = list.ListCustomer.OrderByDescending(x => x.CreatedDate).ToList(),
                Criteria = new CustomerCriteria()
            };

            try
            {
                IExporter exporter = new CustomerDataExcelExporter(this.Response, viewModel.ListCustomer);
                exporter.Export();
            }
            catch
            {
                throw;
            }
            return this.View("IndexCustomer", viewModel);
        }
    }
}