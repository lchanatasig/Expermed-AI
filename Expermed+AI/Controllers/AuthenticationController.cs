using Microsoft.AspNetCore.Mvc;
using Expermed_AI.Services;
using Expermed_AI.Models;
namespace Expermed_AI.Controllers
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }

    public class AuthenticationController : Controller
    {


        private readonly AuthenticationServices _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(AuthenticationServices userService, IHttpContextAccessor httpContextAccessor, ILogger<AuthenticationController> logger)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }


        [HttpGet]
        [ActionName("SignInBasic")]
        public async Task<IActionResult> SignInBasic()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignInBasic([FromForm] string loginUsuario, [FromForm] string claveUsuario)
        {
            if (string.IsNullOrEmpty(loginUsuario) || string.IsNullOrEmpty(claveUsuario))
            {
                // Si las credenciales son inválidas, redirige a la vista SignInBasic sin mensajes
                return RedirectToAction("SignInBasic");
            }

            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            try
            {
                // Intenta validar al usuario
                User user = await _userService.ValidateUser(loginUsuario, claveUsuario, ipAddress);

                if (user != null)
                {
                    // Si el usuario es válido, crea la respuesta de éxito
                    var successResponse = new
                    {
                        success = true,
                        message = "Inicio de sesión exitoso",
                        user,
                        redirectUrl = Url.Action("Home", "DashBoard") // URL para redirigir después del inicio de sesión
                    };

                    // Devuelve una respuesta JSON con el objeto de éxito
                    return Ok(successResponse);
                }
                else
                {
                    // Si las credenciales son inválidas, redirige a SignInBasic
                    return RedirectToAction("SignInBasic");
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Redirige a SignInBasic si hay un acceso no autorizado
                return RedirectToAction("SignInBasic");
            }
            catch (Exception)
            {
                // Redirige a SignInBasic en caso de cualquier otra excepción
                return RedirectToAction("SignInBasic");
            }
        }



        [ActionName("LogoutBasic")]
        public IActionResult LogoutBasic()
        {
            return View();
        }
    }
}
