using ClosedXML.Excel;
using EasyAssetManagerCore.BusinessLogic.Operation;
using EasyAssetManagerCore.BusinessLogic.Security;
using EasyAssetManagerCore.Models.CommonModel;
using EasyAssetManagerCore.Models.EntityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace EasyAssetManager.Controllers
{
    public class SMEController : BaseController
    {
        private IHostingEnvironment environment;
        public SMEController(IHostingEnvironment environment)
        {
            this.environment = environment;
        }
        public IActionResult BranchWiseRetailReport()
        {
            return View();
        }

        public IActionResult Excel()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Users");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Username";
                for (var i=0;i<=50;i++)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = i;
                    worksheet.Cell(currentRow, 2).Value = "50";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "users.xlsx");
                }
            }


        }

    }
    }
