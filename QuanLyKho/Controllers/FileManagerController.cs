using Microsoft.AspNetCore.Mvc;

namespace QuanLyKho.Controllers
{
    [Route("/file-manager")]
    public class FileManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
