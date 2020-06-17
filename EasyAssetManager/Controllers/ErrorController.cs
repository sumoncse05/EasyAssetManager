using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{StatusCode}")]
        public IActionResult StatusCodeHandle(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessasge = $"Page Not Found. {statusCode}" + " Error code Message";
                    break;
                default:
                    ViewBag.ErrorMessasge = $"Permission Not Found. {statusCode}" + " Error code Message";
                    break;

            }

            return View(statusCode);
        }
    }
}