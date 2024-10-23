using Microsoft.AspNetCore.Mvc;

namespace Expermed_AI.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult PatientList()
        {
            return View();
        }
    }
}