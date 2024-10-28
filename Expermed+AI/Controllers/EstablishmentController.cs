using Microsoft.AspNetCore.Mvc;

namespace Expermed_AI.Controllers
{
    public class EstablishmentController : Controller
    {
        public IActionResult EstablishmentList()
        {
            return View();
        }

        [HttpGet("NewEstablishment")]
        public async Task<IActionResult> NewEstablishment()
        {


            return View();

        }


    }
}
