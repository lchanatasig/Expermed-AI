using Microsoft.AspNetCore.Mvc;

namespace Expermed_AI.Controllers
{
    public class UsersController : Controller
    {
        [ActionName("ProfileSimple")]
        public IActionResult ProfileSimple()
        {
            return View();
        }

        [ActionName("UserList")]
        public IActionResult UserList()
        {
            return View();
        }

        [ActionName("NewUser")]
        public IActionResult NewUser()
        {
            return View();
        }
        [ActionName("UpdateUser")]
        public IActionResult UpdateUser()
        {
            return View();
        }
    }
}
