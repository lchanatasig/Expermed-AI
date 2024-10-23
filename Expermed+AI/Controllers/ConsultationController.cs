using Microsoft.AspNetCore.Mvc;

namespace Expermed_AI.Controllers
{
    public class ConsultationController : Controller
    {
        public IActionResult ConsultationList()
        {
            return View();
        }
    }
}
