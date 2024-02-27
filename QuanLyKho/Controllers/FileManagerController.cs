using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QuanLyKho.Controllers
{
    [Authorize(Roles = "Admin,Storekeeper,Manager")]
    [Route("/file-manager")]
    public class FileManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
