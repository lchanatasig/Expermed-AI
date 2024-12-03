using Expermed_AI.Models;
using Expermed_AI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
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

       

        [HttpGet]
        public async Task<IActionResult> NewUser()
        {
            try
            {
                // Consume ambos servicios
                var profiles = await _selectService.GetAllProfilesAsync();
                var specialties = await _selectService.GetAllSpecialtiesAsync();
                var establishment = await _selectService.GetAllEstablishmentsAsync();
                var medics = await _selectService.GetAllMedicsAsync();
                var countries = await _selectService.GetAllCountriesAsync();
                var percentage = await _selectService.GetAllVatPercentageAsync();

                // Crea un ViewModel para pasar ambos conjuntos de datos a la vista
                var viewModel = new NewUserViewModel
                {
                    Profiles = profiles,
                    Specialties = specialties,
                    Establishments = establishment,
                    Users = medics,
                    Countries = countries,
                    VatBillings = percentage,
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


        [HttpPost]
        public async Task<IActionResult> NewUser(UserViewModel usuario, IFormFile? DigitalSignature, IFormFile? ProfilePhoto, int? idMedicoAsociado = null)
        {
            // Verificar si el modelo es válido
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    var key = state.Key; // Nombre del campo
                    var errors = state.Value.Errors; // Lista de errores

                    foreach (var error in errors)
                    {
                        // Registra los errores en un log, consola o TempData
                        Console.WriteLine($"Campo: {key}, Error: {error.ErrorMessage}");
                    }
                }

                TempData["ErrorMessage"] = "Datos inválidos. Por favor, revisa los campos e intenta de nuevo.";
                // Consume ambos servicios
                var profiles = await _selectService.GetAllProfilesAsync();
                var specialties = await _selectService.GetAllSpecialtiesAsync();
                var establishment = await _selectService.GetAllEstablishmentsAsync();
                var medics = await _selectService.GetAllMedicsAsync();
                var countries = await _selectService.GetAllCountriesAsync();
                var percentage = await _selectService.GetAllVatPercentageAsync();

                // Crea un ViewModel para pasar ambos conjuntos de datos a la vista
                var viewModel = new NewUserViewModel
                {
                    Profiles = profiles,
                    Specialties = specialties,
                    Establishments = establishment,
                    Users = medics,
                    Countries = countries,
                    VatBillings = percentage,
                };

                return View(viewModel);
            }

            // Convierte los archivos a byte[] si fueron proporcionados
            usuario.UserDigitalsignature = await ConvertFileToByteArray(DigitalSignature);
            usuario.UserProfilephoto = await ConvertFileToByteArray(ProfilePhoto);

            // Agregar médicos asociados si se proporcionaron
            if (idMedicoAsociado.HasValue)
            {
                // Si hay un médico asociado, puedes incluir la lógica aquí para asignar el ID.
                // En tu servicio ya manejamos el parámetro `idMedicoAsociado`.
                // Si tienes múltiples médicos, asegúrate de manejarlos correctamente.
            }

            try
            {
                // Llamar al servicio para crear el usuario, pasando el médico asociado si aplica
                int idUsuarioCreado = await _usersService.CreateUserAsync(usuario, idMedicoAsociado);

                // Si el proceso de creación fue exitoso
                TempData["SuccessMessage"] = "Usuario creado exitosamente.";
                return RedirectToAction("ListarUsuarios", "User");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones personalizado
                if (ex.Message.Contains("El usuario con este CI ya existe."))
                {
                    TempData["ErrorMessage"] = "El usuario con este CI ya existe.";
                }
                else if (ex.Message.Contains("El nombre de usuario ya existe."))
                {
                    TempData["ErrorMessage"] = "El nombre de usuario ya existe.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error inesperado: " + ex.Message;
                }

                // Cargar listas para re-renderizar la vista en caso de error
                // Consume ambos servicios
                var profiles = await _selectService.GetAllProfilesAsync();
                var specialties = await _selectService.GetAllSpecialtiesAsync();
                var establishment = await _selectService.GetAllEstablishmentsAsync();
                var medics = await _selectService.GetAllMedicsAsync();
                var countries = await _selectService.GetAllCountriesAsync();
                var percentage = await _selectService.GetAllVatPercentageAsync();

                // Crea un ViewModel para pasar ambos conjuntos de datos a la vista
                var viewModel = new NewUserViewModel
                {
                    Profiles = profiles,
                    Specialties = specialties,
                    Establishments = establishment,
                    Users = medics,
                    Countries = countries,
                    VatBillings = percentage,
                };

                return View(viewModel); 
            }
        }


        //METODO PARA CONVERTIR ARCHIVOS
        private async Task<byte[]> ConvertFileToByteArray(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            return null;
        }


        [ActionName("UpdateUser")]
        public async Task<IActionResult> UpdateUser(int id)
        {
            // Get the user details
            var user = await _usersService.GetUserByIdAsync(id);

            // If the user does not exist, return a not found response
            if (user == null)
            {
                return NotFound("User Not Found");
            }

            // Get the lists of profiles, specialties, establishments, and medics
            var profiles = await _selectService.GetAllProfilesAsync();
            var specialties = await _selectService.GetAllSpecialtiesAsync();
            var establishments = await _selectService.GetAllEstablishmentsAsync();
            var medics = await _selectService.GetAllMedicsAsync();

            // Create a ViewModel to pass both the user and the lists to the view
            var viewModel = new NewUserViewModel
            {
                User = user,  // Pass the user object to the ViewModel
                Profiles = profiles,
                Specialties = specialties,
                Establishments = establishments,
                Users = medics
            };

            // Return the view with the populated ViewModel
            return View(viewModel);
        }


        // Clase para la solicitud del endpoint
        public class CreateUserRequest
        {
            public byte[] ProfilePhoto { get; set; }
            public string ProfilePhoto64 { get; set; }
            public int ProfileId { get; set; }
            public string DocumentNumber { get; set; }
            public string Names { get; set; }
            public string Surnames { get; set; }
            public string Address { get; set; }
            public string SenecytCode { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public int? SpecialtyId { get; set; }
            public int? CountryId { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public int? EstablishmentId { get; set; }
            public int? VatPercentageId { get; set; }
            public string XKeyTaxo { get; set; }
            public string XPassTaxo { get; set; }
            public int? SequentialBilling { get; set; }
            public byte[] DigitalSignature { get; set; }
            public string DoctorIds { get; set; }
            public TimeSpan? StartTime { get; set; }
            public TimeSpan? EndTime { get; set; }
            public string WorkDays { get; set; }
            public int? AppointmentInterval { get; set; }
            public string Description { get; set; }
        }





    }
}
