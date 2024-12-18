using Expermed_AI.Models;
using Expermed_AI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Expermed_AI.Controllers
{
    public class PatientController : Controller
    {
        private readonly UsersService _usersService;
        private readonly ILogger<UsersController> _logger;
        private readonly SelectsService _selectService;

        // Inyección de dependencias
        public PatientController(UsersService usersService, ILogger<UsersController> logger, SelectsService selectService)
        {
            _usersService = usersService;
            _logger = logger;
            _selectService = selectService;
        }

        public IActionResult PatientList()
        {
            return View();
        }





        [HttpGet]
        public async Task<IActionResult> NewPatient()
        {
            try
            {
                // Obtener los tipos de género
                var genderTypes = await _selectService.GetGenderTypeAsync(); // Asumiendo que tienes un servicio _catalogService

                // Obtener los tipos de sangre
                var bloodTypes = await _selectService.GetBloodTypeAsync(); // Asumiendo que tienes un servicio _catalogService

                // Obtener los tipos de documentos
                var documentTypes = await _selectService.GetDocumentTypeAsync(); // Asumiendo que tienes un servicio _catalogService

                // Obtener los tipos de estado civil
                var civilTypes = await _selectService.GetCivilTypeAsync(); // Asumiendo que tienes un servicio _catalogService

                // Obtener los tipos de formación profesional
                var professionalTrainingTypes = await _selectService.GetProfessionaltrainingTypeAsync(); // Asumiendo que tienes un servicio _catalogService

                // Obtener los tipos de seguros de salud
                var sureHealthTypes = await _selectService.GetSureHealtTypeAsync(); // Asumiendo que tienes un servicio _catalogService
                // Obtener las nacionalidades
                var countries = await _selectService.GetAllCountriesAsync();

                var provinces = await _selectService.GetAllProvinceAsync();


                // Crear un objeto de vista modelo para pasar los datos a la vista
                var viewModel = new NewPatientViewModel
                {
                    GenderTypes = genderTypes,
                    BloodTypes = bloodTypes,
                    DocumentTypes = documentTypes,
                    CivilTypes = civilTypes,
                    ProfessionalTrainingTypes = professionalTrainingTypes,
                    SureHealthTypes = sureHealthTypes,
                    Countries = countries,
                    Provinces = provinces
 
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Manejo de excepciones personalizado
                TempData["ErrorMessage"] = "Error inesperado: " + ex.Message;
                return View();
            }
        }

    }
}