using Microsoft.AspNetCore.Mvc;
namespace ITWEB
{
    public class IndexIT : Controller
    {
        public IActionResult Index()
        {
            return View("IndexIT.cshtml");
        }
    }
}
