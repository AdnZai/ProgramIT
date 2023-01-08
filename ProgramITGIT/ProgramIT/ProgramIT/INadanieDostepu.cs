using Microsoft.AspNetCore.Mvc;
namespace ITWEB
{
    public class INadanieDostepu : Controller
    {
        public IActionResult Index()
        {
            return View("INadanieDostepu.cshtml");
        }
    }
}
