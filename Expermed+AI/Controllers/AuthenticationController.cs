using Microsoft.AspNetCore.Mvc;

namespace Expermed_AI.Controllers
{
    public class AuthenticationController : Controller
    {
        [ActionName("SignInBasic")]
        public IActionResult SignInBasic()
        {
            return View();
        }

        [ActionName("LogoutBasic")]
        public IActionResult LogoutBasic()
        {
            return View();
        }
    }
}
