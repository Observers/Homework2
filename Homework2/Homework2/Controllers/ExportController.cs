using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Data;
using System.IO;
using System.Web.Mvc;

// Resource : https://www.aspsnippets.com/Articles/Convert-HTML-Table-to-DataTable-in-ASPNet-using-C-and-VBNet.aspx
// Resource : https://techbrij.com/export-excel-xls-xlsx-asp-net-npoi-epplus
// The functions below are the combination of the above resources. Slight changes were made to the code for this to work.
namespace Homework2.Controllers
{
    public class ExportController : Controller
    {
        // Download function to receive JSON to convert to excel file.
        [HttpPost]
        public void Submit(object sender, EventArgs e)
        {
                string JSON = Request.Form["JSON"];
            System.Diagnostics.Debug.WriteLine(JSON);
                DataTable dt = JsonConvert.DeserializeObject<DataTable>(JSON);
                WriteExcelWithNPOI(dt, "xlsx");
        }

        // Write JSON data to excel file and send excel document to user to download.
        public void WriteExcelWithNPOI(DataTable dt, String extension)
        {

            IWorkbook workbook;

            if (extension == "xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if (extension == "xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                throw new Exception("This format is not supported");
            }

            ISheet sheet1 = workbook.CreateSheet("Sheet 1");

            // Make a header row
            IRow row1 = sheet1.CreateRow(0);

            for (int j = 0; j < dt.Columns.Count; j++)
            {

                ICell cell = row1.CreateCell(j);
                String columnName = dt.Columns[j].ToString();
                cell.SetCellValue(columnName);
            }

            // Loops through data
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row = sheet1.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    ICell cell = row.CreateCell(j);
                    String columnName = dt.Columns[j].ToString();
                    cell.SetCellValue(dt.Rows[i][columnName].ToString());
                }
            }

            // Steps to send excel file to client to download.
            using (var exportData = new MemoryStream())
            {
                Response.Clear();
                workbook.Write(exportData);
                if (extension == "xlsx") //xlsx file format
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "ContactNPOI.xlsx"));
                    Response.BinaryWrite(exportData.ToArray());
                }
                else if (extension == "xls")  //xls file format
                {
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "ContactNPOI.xls"));
                    Response.BinaryWrite(exportData.GetBuffer());
                }
                Response.End();
            }
        }
    }
}