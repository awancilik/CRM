using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace ISAT.Web.Helpers
{
    public class HtmlToExcelDashboardHelper
    {
        public static void ExportFromGridView(HttpResponseBase httpResponse, Dictionary<string, string> fields, IEnumerable data, string targetFileName, bool headerData = true)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            System.Web.UI.WebControls.GridView gv = new System.Web.UI.WebControls.GridView();

            try
            {
                foreach (var col in fields)
                {
                    var column = new System.Web.UI.WebControls.BoundField();
                    column.DataField = col.Key;
                    column.HeaderText = col.Value;
                    gv.Columns.Add(column);
                }

                gv.AutoGenerateColumns = false;
                gv.DataSource = data;
                gv.DataBind();

                // add timestamp to file name (only if '{timestamp}' exist in filename)
                var fileName = targetFileName.Replace("{timestamp}", DateTime.Now.ToString("yyyyMMddHHmmss"));

                httpResponse.ClearContent();
                httpResponse.Buffer = true;
                httpResponse.AddHeader("content-disposition", $"attachment; filename={fileName}");
                httpResponse.ContentType = "application/vnd.ms-excel";
                httpResponse.Charset = string.Empty;

                foreach (System.Web.UI.WebControls.GridViewRow r in gv.Rows)
                {
                    if (r.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
                    {
                        if (headerData)
                        {
                            //r.Cells[r.Cells.Count - 1].BackColor = ((r.Cells[r.Cells.Count - 1].Text == "green") ? System.Drawing.Color.Green : (((r.Cells[r.Cells.Count - 1].Text == "red") ? System.Drawing.Color.Red : System.Drawing.Color.Yellow)));
                            //r.Cells[r.Cells.Count - 1].Text = string.Empty;
                        }
                        else
                        {
                            r.Cells[r.Cells.Count - 2].BackColor = ((r.Cells[r.Cells.Count - 2].Text == "green") ? System.Drawing.Color.Green : (((r.Cells[r.Cells.Count - 2].Text == "red") ? System.Drawing.Color.Red : System.Drawing.Color.Yellow)));
                            r.Cells[r.Cells.Count - 2].Text = string.Empty;
                            r.Cells[r.Cells.Count - 1].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                        }
                    }
                }

                gv.RenderControl(htw);

                httpResponse.Output.Write(sw.ToString());
                httpResponse.Flush();
                httpResponse.End();
            }
            finally
            {
                // cleanup
                sw = null;
                htw = null;
                gv = null;
            }
        }
    }
}