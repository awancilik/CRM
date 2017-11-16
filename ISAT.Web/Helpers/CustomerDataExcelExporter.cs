using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISAT.Business;

namespace ISAT.Web.Helpers
{
    public class CustomerDataExcelExporter : IExporter
    {
        private const string TargetFileName = "CustomerDataExcel{timestamp}.xls";

        private HttpResponseBase httpResponse;
        private List<Customer> customerData;

        public CustomerDataExcelExporter(HttpResponseBase response, IEnumerable<Customer> data)
        {
            try
            {
                this.httpResponse = response;
                this.customerData = (List<Customer>)data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Export()
        {
            var data = from a in this.customerData
                       select new
                       {
                           FirstName = a.FirstName,
                           LastName = a.LastName,
                           Email = a.Email,
                           CustNo = a.CustNo,
                           CompanyName = a.CompanyName,
                           CreatedDate = a.CreatedDate,
                           CreatedBy = a.CreatedBy
                       };

            System.Web.UI.WebControls.GridView gv = new System.Web.UI.WebControls.GridView();

            Dictionary<string, string> fields = new Dictionary<string, string>();
            fields.Add("FirstName", "FirstName");
            fields.Add("LastName", "LastName");
            fields.Add("Email", "Email");
            fields.Add("CustNo", "CustNo");
            fields.Add("CompanyName", "CompanyName");
            fields.Add("CreatedDate", "CreatedDate");
            fields.Add("CreatedBy", "CreatedBy");

            HtmlToExcelDashboardHelper.ExportFromGridView(this.httpResponse, fields, data, TargetFileName);
        }
    }
}