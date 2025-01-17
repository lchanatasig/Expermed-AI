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
                User user = await _userService.ValidateUser(loginUsuario, claveUsuario, ipAddress);

                return Ok(new
                {
                    success = true,
                    message = "Welcome to our system, Expermed wishes you a good day",
                    redirectUrl = "/DashBoard/Home"
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("UnauthorizedAccessException: {Message}, StackTrace: {StackTrace}", ex.Message, ex.StackTrace);

                return Unauthorized(new
                {
                    success = false,
                    message = ex.Message,
                    exceptionType = ex.GetType().Name // Tipo de excepción
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("ArgumentException: {Message}, StackTrace: {StackTrace}", ex.Message, ex.StackTrace);

                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    exceptionType = ex.GetType().Name // Tipo de excepción
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred during login.");

                return StatusCode(500, new
                {
                    success = false,
                    message = "An unexpected error occurred.",
                    detailedMessage = ex.Message, // Mensaje del error
                    exceptionType = ex.GetType().Name, // Tipo de excepción
                    stackTrace = ex.StackTrace // Trazabilidad completa
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
