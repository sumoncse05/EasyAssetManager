using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageAPIController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public ImageAPIController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("[action]/{customerId}")]
        [HttpGet]
        public string GetCustomerRegistrationImage(string customerId)
        {
            try
            {
                string path = Path.Combine(_hostingEnvironment.WebRootPath, "photo\\" + customerId + ".jpg");
                byte[] b = System.IO.File.ReadAllBytes(path);
                return "data:image/png;base64," + Convert.ToBase64String(b);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [Route("[action]/{transactionId}")]
        [HttpGet]
        public string GetCustomerRemittanceImage(string transactionId)
        {
            try
            {

                string path = Path.Combine(_hostingEnvironment.WebRootPath, "RemittancePhoto\\" + transactionId + ".jpg");
                byte[] b = System.IO.File.ReadAllBytes(path);
                return "data:image/png;base64," + Convert.ToBase64String(b);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}
