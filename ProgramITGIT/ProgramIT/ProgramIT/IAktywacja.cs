using Microsoft.AspNetCore.Mvc;
namespace ITWEB
{
    public class IAktywacja : Controller
    {
        public IActionResult Index()
        {
            return View("IAktywacja.cshtml");
        }
    }
}
