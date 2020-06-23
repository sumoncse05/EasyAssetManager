using System.IO;
using EasyAssetManagerCore.BusinessLogic.Operation.Asset;
using EasyAssetManagerCore.Model.CommonModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class FileProcessController : BaseController
    {
        private IFileProcessManager fileProcessManager;
        private IHostingEnvironment environment;
        private readonly IHttpContextAccessor contextAccessor;
        public FileProcessController(IHostingEnvironment environment, IHttpContextAccessor contextAccessor, IFileProcessManager fileProcessManager)
        {
            this.environment = environment;
            this.contextAccessor = contextAccessor;
            this.fileProcessManager = fileProcessManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(int businessYear, int file_Type, IFormFile file)
        {
            var message = new Message();
            if (file != null)
            {
               var filepath = Path.Combine(environment.WebRootPath, "UserSpace") + $@"\{Session.User.user_id}" + "\\"+ file.FileName;
                var directory = Path.Combine(environment.WebRootPath, "UserSpace") + $@"\{Session.User.user_id}";
                if (!string.IsNullOrEmpty(filepath))
                {
                    if (System.IO.File.Exists(Path.Combine(filepath)))
                        System.IO.File.Delete(Path.Combine(filepath));
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);
                    StoreInFolder(file, filepath);
                    message= fileProcessManager.ProcessFile(businessYear,file_Type, filepath, Session);
                }
                else
                {
                    MessageHelper.Error(message, "Directory not found...");
                }

            }
            else
            {
                MessageHelper.Error(message, "No file uploaded...");
            }

            return Json(message);
        }

        private void StoreInFolder(IFormFile file, string fileName)
        {
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }
    }
}