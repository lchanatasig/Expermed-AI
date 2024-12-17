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
        public async Task<IActionResult> NewUser(UserViewModel usuario, IFormFile? DigitalSignature, IFormFile? ProfilePhoto, string? selectedDoctorIds, string selectedWorkDays)
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

                // Consume ambos servicios para recargar datos
                var profiles = await _selectService.GetAllProfilesAsync();
                var specialties = await _selectService.GetAllSpecialtiesAsync();
                var establishment = await _selectService.GetAllEstablishmentsAsync();
                var medics = await _selectService.GetAllMedicsAsync();
                var countries = await _selectService.GetAllCountriesAsync();
                var percentage = await _selectService.GetAllVatPercentageAsync();

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

            // Procesa los IDs de médicos seleccionados
            List<int>? associatedDoctorIds = null;
            if (!string.IsNullOrEmpty(selectedDoctorIds))
            {
                // Convierte la cadena separada por comas en una lista de enteros
                associatedDoctorIds = selectedDoctorIds
                    .Split(',')
                    .Select(id => int.Parse(id))
                    .ToList();
            }
            // Procesa los IDs de médicos seleccionados
            var workDays = selectedWorkDays?.Split(',').ToList();


            try
            {
                // Llamar al servicio para crear el usuario y asociar los médicos
                int idUsuarioCreado = await _usersService.CreateUserAsync(usuario, associatedDoctorIds,workDays);

                // Si el proceso de creación fue exitoso
                TempData["SuccessMessage"] = "User created successfully.";
                return RedirectToAction("UserList", "Users");
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

                // Consume ambos servicios para recargar datos
                var profiles = await _selectService.GetAllProfilesAsync();
                var specialties = await _selectService.GetAllSpecialtiesAsync();
                var establishment = await _selectService.GetAllEstablishmentsAsync();
                var medics = await _selectService.GetAllMedicsAsync();
                var countries = await _selectService.GetAllCountriesAsync();
                var percentage = await _selectService.GetAllVatPercentageAsync();

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


        [HttpGet]
        public async Task<IActionResult> UpdateUser(int id)
        {
            // Obtener los detalles del usuario (incluyendo médicos si es asistente)
            var user = await _usersService.GetUserDetailsAsync(id);

            // Si el usuario no existe, devolver una respuesta de "No encontrado"
            if (user == null)
            {
                return NotFound("User Not Found");
            }

            // Obtener las listas de perfiles, especialidades, establecimientos y médicos
            var profiles = await _selectService.GetAllProfilesAsync();
            var specialties = await _selectService.GetAllSpecialtiesAsync();
            var establishments = await _selectService.GetAllEstablishmentsAsync();
            var countries = await _selectService.GetAllCountriesAsync();
            var percentage = await _selectService.GetAllVatPercentageAsync();

            var medics = await _selectService.GetAllMedicsAsync();

            // Crear un ViewModel para pasar tanto el usuario como las listas a la vista
            var viewModel = new NewUserViewModel
            {
                User = user,  // Pasar el usuario obtenido al ViewModel
                Profiles = profiles,
                Specialties = specialties,
                Establishments = establishments,
                Countries = countries,
                Users = medics,
                VatBillings = percentage,
                AssociatedDoctors = user.Doctors // Incluir los médicos asociados si es asistente

            };

            // Devolver la vista con el ViewModel poblado
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserViewModel usuario, IFormFile? DigitalSignature, IFormFile? ProfilePhoto, string? selectedDoctorIds, string selectedWorkDays,int id)
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
                // Obtener los detalles del usuario (incluyendo médicos si es asistente)
                var user = await _usersService.GetUserDetailsAsync(id);

                // Si el usuario no existe, devolver una respuesta de "No encontrado"
                if (user == null)
                {
                    return NotFound("User Not Found");
                }
                // Consume ambos servicios para recargar datos
                
                var profiles = await _selectService.GetAllProfilesAsync();
                var specialties = await _selectService.GetAllSpecialtiesAsync();
                var establishment = await _selectService.GetAllEstablishmentsAsync();
                var medics = await _selectService.GetAllMedicsAsync();
                var countries = await _selectService.GetAllCountriesAsync();
                var percentage = await _selectService.GetAllVatPercentageAsync();

                var viewModel = new NewUserViewModel
                {
                    User = user,
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

            // Procesa los IDs de médicos seleccionados
            List<int>? associatedDoctorIds = null;
            if (!string.IsNullOrEmpty(selectedDoctorIds))
            {
                associatedDoctorIds = selectedDoctorIds
                    .Split(',')
                    .Where(id => int.TryParse(id, out _)) // Verifica si cada ID puede ser convertido a un entero
                    .Select(id => int.Parse(id))
                    .ToList();
            }

            // Procesa los días de trabajo seleccionados
            var workDays = selectedWorkDays?.Split(',').ToList();

            try
            {
                // Llama al servicio para actualizar el usuario
                await _usersService.UpdateUserAsync(usuario, associatedDoctorIds, workDays);

                // Si el proceso de actualización fue exitoso
                TempData["SuccessMessage"] = "User updated successfully.";
                return RedirectToAction("UserList", "Users");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones personalizado
                if (ex.Message.Contains("El usuario no existe."))
                {
                    TempData["ErrorMessage"] = "El usuario no existe.";
                }
                else if (ex.Message.Contains("El número de documento ya existe."))
                {
                    TempData["ErrorMessage"] = "El número de documento ya existe.";
                }
                else if (ex.Message.Contains("El nombre de usuario ya está registrado."))
                {
                    TempData["ErrorMessage"] = "El nombre de usuario ya está registrado.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error inesperado: " + ex.Message;
                }

                // Obtener los detalles del usuario (incluyendo médicos si es asistente)
                var user = await _usersService.GetUserDetailsAsync(id);

                // Si el usuario no existe, devolver una respuesta de "No encontrado"
                if (user == null)
                {
                    return NotFound("User Not Found");
                }
                var profiles = await _selectService.GetAllProfilesAsync();
                var specialties = await _selectService.GetAllSpecialtiesAsync();
                var establishment = await _selectService.GetAllEstablishmentsAsync();
                var medics = await _selectService.GetAllMedicsAsync();
                var countries = await _selectService.GetAllCountriesAsync();
                var percentage = await _selectService.GetAllVatPercentageAsync();

                var viewModel = new NewUserViewModel
                {
                    User = user,
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


    }
}
