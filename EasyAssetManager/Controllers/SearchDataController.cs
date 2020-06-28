using Microsoft.AspNetCore.Mvc;

namespace EasyAssetManager.Controllers
{
    public class SearchDataController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}