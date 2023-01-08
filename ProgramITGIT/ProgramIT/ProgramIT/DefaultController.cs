using Microsoft.AspNetCore.Mvc;

namespace ProgramIT
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View("Index.cshtml");
        }
    }
}



