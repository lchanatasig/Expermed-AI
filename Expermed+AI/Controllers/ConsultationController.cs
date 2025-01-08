using Expermed_AI.Services;
using Microsoft.AspNetCore.Mvc;
using Expermed_AI.Models;

namespace Expermed_AI.Controllers
{
    public class ConsultationController : Controller
    {
        private readonly SelectsService _selectService;
        private readonly PatientService _patientService;
        private readonly AppointmentService _appointmentService;
        // Inyección de dependencias
        public ConsultationController(SelectsService selectService,PatientService patientService, AppointmentService appointmentService)
        {
            _selectService = selectService;
            _patientService = patientService;
            _appointmentService = appointmentService;
        }

        public IActionResult ConsultationList()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> NewConsultation(int patientId)
        {
            try
            {
                // Obtener los detalles del paciente
                var patient = await _patientService.GetPatientDetailsAsync(patientId);

                // Si el paciente no existe, devolver una respuesta de "No encontrado"
                if (patient == null)
                {
                    return NotFound("Patient not found.");
                }

                // Obtener datos adicionales para la vista
                var genderTypes = await _selectService.GetGenderTypeAsync();
                var bloodTypes = await _selectService.GetBloodTypeAsync();
                var documentTypes = await _selectService.GetDocumentTypeAsync();
                var civilTypes = await _selectService.GetCivilTypeAsync();
                var professionalTrainingTypes = await _selectService.GetProfessionaltrainingTypeAsync();
                var sureHealthTypes = await _selectService.GetSureHealtTypeAsync();
                var countries = await _selectService.GetAllCountriesAsync();
                var provinces = await _selectService.GetAllProvinceAsync();

                // Crear el ViewModel
                var viewModel = new NewPatientViewModel
                {
                    Patient = patient,
                    GenderTypes = genderTypes,
                    BloodTypes = bloodTypes,
                    DocumentTypes = documentTypes,
                    CivilTypes = civilTypes,
                    ProfessionalTrainingTypes = professionalTrainingTypes,
                    SureHealthTypes = sureHealthTypes,
                    Countries = countries,
                    Provinces = provinces

                };

                // Retornar la vista con el modelo
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error inesperado: " + ex.Message;
                return View();
            }
        }


    }
}
