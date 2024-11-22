using Expermed_AI.Models;
using Expermed_AI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Expermed_AI.Controllers
{
    public class UsersController : Controller
    {
        private readonly UsersService _usersService;
        private readonly ILogger<UsersController> _logger;
        private readonly SelectsService _selectService;

        // Inyección de dependencias
        public UsersController(UsersService usersService, ILogger<UsersController> logger, SelectsService selectService)
        {
            _usersService = usersService;
            _logger = logger;
            _selectService = selectService;
        }

        [ActionName("ProfileSimple")]
        public IActionResult ProfileSimple()
        {
            return View();
        }

        [ActionName("UserList")]
        public async Task<IActionResult> UserList()
        {
            try
            {
                // Obtiene todos los usuarios usando el servicio
                var users = await _usersService.GetAllUsersAsync();
                

                // Pasa los usuarios a la vista
                return View(users);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                _logger.LogError(ex, "Error getting list of users.");
                return View("Error"); // O una vista de error personalizada
            }
        }

        // Acción para activar o desactivar un usuario
        [HttpPost]
        public async Task<IActionResult> ChangeUserStatus(int userId, int status)
        {
            var result = await _usersService.DesactiveOrActiveUserAsync(userId, status);

            if (result.success)
            {
                TempData["SuccessMessage"] = result.message; // Mensaje de éxito
            }
            else
            {
                TempData["ErrorMessage"] = result.message; // Mensaje de error
            }

            return RedirectToAction("UserList"); // Redirigir a la lista de usuarios
        }

       

        [ActionName("NewUser")]
        public async Task<IActionResult> NewUser()
        {
            try
            {
                // Consume ambos servicios
                var profiles = await _selectService.GetAllProfilesAsync();
                var specialties = await _selectService.GetAllSpecialtiesAsync();
                var establishment = await _selectService.GetAllEstablishmentsAsync();
                var medics = await _selectService.GetAllMedicsAsync();

                // Crea un ViewModel para pasar ambos conjuntos de datos a la vista
                var viewModel = new NewUserViewModel
                {
                    Profiles = profiles,
                    Specialties = specialties,
                    Establishments = establishment,
                    Users = medics
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Manejo de errores, por ejemplo, redirigir a una página de error o mostrar un mensaje
                _logger.LogError(ex, "Error al cargar datos para NewUser.");
                return View("Error"); // Asegúrate de tener una vista "Error"
            }
        }

        [ActionName("UpdateUser")]
        public async Task<IActionResult> UpdateUser(int id)
        {
            // Usamos el servicio para obtener los detalles del usuario por su ID
            var user = await _usersService.GetUserByIdAsync(id);

            // Si el usuario no existe, puedes redirigir a una página de error o mostrar un mensaje
            if (user == null)
            {
                return NotFound("El usuario no fue encontrado");
            }

            // Devolvemos la vista con el modelo del usuario
            return View(user);
        }


    }
}
