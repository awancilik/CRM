using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISAT.Business;
using OfficeOpenXml;
using System.IO;
using System.Text;


namespace ISAT.Web.Controllers
{
    [Authorize]
    public class ExportController : Controller
    {
        // GET: Export
        public ActionResult IndexExport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndexExport(int month, int year, string ticketSubject)
        {
            try
            {
                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);
                TicketInfoList list = TicketInfoList.GetReportByDate(startDate, endDate, ticketSubject);

                var file = CreateFile(list, month, year, ticketSubject);
                if (file != null)
                {
                    return new FileContentResult(file,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    { FileDownloadName = "My Excel File.xlsx" };
                }
            }
            catch (Exception ex)
            {
                LogActivity log = LogActivity.NewLogActivity();
                log.ControllerName = "ExportController";
                log.ActionName = "IndexExport";
                log.LogData = Newtonsoft.Json.JsonConvert.SerializeObject(ex.StackTrace);
                log.CreatedBy = User.Identity.Name;
                log.CreatedDate = DateTime.Now;
                log = log.Save();
            }
            return View();
        }

        private byte[] CreateFile(TicketInfoList list, int month, int year, string ticketSubject)
        {
            string filename = "Export File.xlsx";
            var filetemplate = new FileInfo(ControllerContext.HttpContext.Server.MapPath("~/Content/template/LAPORAN BULAN.xlsx"));
            var newfile = new FileInfo(ControllerContext.HttpContext.Server.MapPath("~/Content/Download/" + filename));

            string bulan = new DateTime(year, month, 1).ToString("MMM");
            string tahun = new DateTime(year, month, 1).ToString("yyyy");
            int startrow = 8;
            var ms = new MemoryStream();

            using (ExcelPackage newexcel = new ExcelPackage(ms))
            {
                var wb = newexcel.Workbook;
                ExcelWorksheet ws = wb.Worksheets.Add("Sheet1");
                using (ExcelPackage templateexcel = new ExcelPackage(filetemplate))
                {
                    ExcelWorksheet wsTemplate = templateexcel.Workbook.Worksheets[1];

                    ws.Cells[2, 1].Value = wsTemplate.Cells[2, 1].Value.ToString().Replace("[TANGGAL_TAHUN]", bulan.ToUpper() + " " + tahun);
                    ws.Cells[2, 1].Style.Font.Bold = true;
                    ws.Cells[2, 1, 2, 9].Merge = true;
                    ws.Cells[2, 1, 2, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[3, 1].Value = wsTemplate.Cells[3, 1].Value;
                    ws.Cells[3, 1].Style.Font.Bold = true;
                    ws.Cells[3, 1, 3, 9].Merge = true;
                    ws.Cells[3, 1, 3, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[4, 1].Value = wsTemplate.Cells[4, 1].Value;
                    ws.Cells[4, 1].Style.Font.Bold = true;
                    ws.Cells[4, 1, 4, 9].Merge = true;
                    ws.Cells[4, 1, 4, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[5, 1].Value = wsTemplate.Cells[5, 1].Value;
                    ws.Cells[5, 1].Style.Font.Bold = true;
                    ws.Cells[5, 1, 5, 9].Merge = true;
                    ws.Cells[5, 1, 5, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[6, 1].Value = wsTemplate.Cells[6, 1].Value;
                    ws.Cells[6, 1].Style.Font.Bold = true;
                    ws.Cells[6, 1, 6, 9].Merge = true;
                    ws.Cells[6, 1, 6, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells[8, 10].Value = "Status";
                    ws.Cells[8, 11].Value = "Durasi";

                    for (int i = 1; i < 10; i++)
                    {
                        ws.Cells[startrow, i].Value = wsTemplate.Cells[startrow, i].Value;
                    }
                }
                startrow = 9;
                foreach (var item in list)
                {
                    ws.Cells[startrow, 1].Value = item.CreatedDate.ToString("dd MMM yyyy");
                    ws.Cells[startrow, 2].Value = item.TicketNo;

                    CustomerInfo info = CustomerInfo.GetCustomerInfo(item.Requester);
                    if (info != null)
                    {
                        ws.Cells[startrow, 3].Value = info.CompanyName;
                    }

                    ws.Cells[startrow, 4].Value = item.CallerPosistion;
                    ws.Cells[startrow, 5].Value = item.TicketType;
                    ws.Cells[startrow, 6].Value = item.Priority;
                    ws.Cells[startrow, 7].Value = item.TicketDescription;
                    ws.Cells[startrow, 7].Style.WrapText = true;

                    List<ISAT.Web.ViewModel.TicketSolution> solutions = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ISAT.Web.ViewModel.TicketSolution>>(item.Solution);
                    StringBuilder sb = new StringBuilder();
                    foreach (var sol in solutions)
                    {
                        sb.AppendLine(sol.Solution);
                    } 
                    ws.Cells[startrow, 8].Value = sb.ToString();
                    ws.Cells[startrow, 8].Style.WrapText = true;

                    ws.Cells[startrow, 9].Value = item.CreatedBy;
                    ws.Cells[startrow, 10].Value = item.TicketStatus;
                    if (item.TicketStatus == TicketStatus.Open.ToString())
                    {
                        DateTime currentDate = DateTime.Now;
                        int days = currentDate.Subtract(item.CreatedDate).Days;
                        int hours = currentDate.Subtract(item.CreatedDate).Hours;

                        string durasi = string.Format("{0} hari {1} jam", days, hours);
                        ws.Cells[startrow, 11].Value = durasi;
                    }


                    startrow++;
                }

                for (int i = 1; i < 11 && i != 8 && i != 7; i++)
                {
                    ws.Column(i).AutoFit();
                }

                newexcel.Save();
                return ms.ToArray();
            }
        }
    }
}