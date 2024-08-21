using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using WebApplication1.Data;

namespace WebApplication1.Controllers.Excel
{
    public class ExcelController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        public ExcelController(IConfiguration configuration, ApplicationDbContext context)
        {
            this.configuration = configuration;
        }
        public IActionResult Index()
        {
            var data = context.Customers.ToList();
            return View();
        }

        public IActionResult ImportExcelFile()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ImportExcelFile(IFormFile formFile)
        {
            try
            {
                    var mainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadExcelFile");
                    if (!Directory.Exists(mainPath))
                    {
                        Directory.CreateDirectory(mainPath);
                    }
                    var filePath = Path.Combine(mainPath, formFile.FileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }
                    var fileName = formFile.FileName;
                    string extension = Path.GetExtension(fileName);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls":
                            conString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filePath + "; Extended Properties='Excel 8.0;HDR=Yes'";
                            break;
                        case ".xlsx":
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filePath + "; Extended Properties='Excel 8.0;HDR=Yes'";
                            break;

                    }
                    DataTable dt = new DataTable();
                    conString = string.Format(conString, filePath);
                    using (OleDbConnection conExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = conExcel;
                                conExcel.Open();
                                DataTable dtExcelSchema = conExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE NAME"].ToString();
                                cmdExcel.CommandText = "SELECT * FROM [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                conExcel.Close();

                            }
                        }
                    }
                    conString = configuration.GetConnectionString("DefaultConnection");
                    using (SqlConnection con = new SqlConnection(conString))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {
                            sqlBulkCopy.DestinationTableName = "Customers";
                            sqlBulkCopy.ColumnMappings.Add("CustomerCode", "CustomerCode");
                            sqlBulkCopy.ColumnMappings.Add("FirstName", "FirstName");
                            sqlBulkCopy.ColumnMappings.Add("LastName", "LastName");
                            sqlBulkCopy.ColumnMappings.Add("Gender", "Gender");
                            sqlBulkCopy.ColumnMappings.Add("Country", "Country");
                            sqlBulkCopy.ColumnMappings.Add("Age", "Age");
                            con.Open();
                            sqlBulkCopy.WriteToServer(dt);
                            con.Close();
                        }
                    }
                    ViewBag.message = "File Imported Successfully,Data saved into Database";
                    return RedirectToAction("Index");
                }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return View();
         }
	}
}
