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
                return BadRequest(new
                {
                    success = false,
                    message = "El usuario y la contraseña no pueden estar vacíos."
                });
            }

            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            try
            {
                // Intenta validar al usuario
                User user = await _userService.ValidateUser(loginUsuario, claveUsuario, ipAddress);

                if (user != null)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Welcome to our system, Expermed wishes you a good day",
                        redirectUrl = Url.Action("Home", "DashBoard")
                    });
                }
                else
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Credenciales inválidas o usuario inactivo."
                    });
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // Mensaje específico para credenciales inválidas
                return Unauthorized(new
                {
                    success = false,
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Loguea el error para diagnóstico
                _logger.LogError(ex, "Ocurrió un error inesperado durante el inicio de sesión.");

                // Mensaje genérico para errores desconocidos
                return StatusCode(500, new
                {
                    success = false,
                    message = "Ocurrió un error inesperado. Por favor, intente nuevamente más tarde."
                });
            }
        }



        [ActionName("LogoutBasic")]
        public IActionResult LogoutBasic()
        {
            return View();
        }
    }
}
