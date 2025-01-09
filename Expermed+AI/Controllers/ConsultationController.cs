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
                var patient = await _patientService.GetPatientDataByIdAsync(patientId);

                // Si el paciente no existe, devolver una respuesta de "No encontrado"
                if (patient == null)
                {
                    TempData["ErrorMessage"] = "Patient not found.";
                    return RedirectToAction("Index", "Home");
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
                var parents = await _selectService.GetRelationshipTypeAsync();
                var allergies = await _selectService.GetAllergiesTypeAsync();
                var surgeries = await _selectService.GetSurgeriesTypeAsync();
                var familyMember = await _selectService.GetFamiliarTypeAsync();


                // Crear el ViewModel
                var viewModel = new NewPatientViewModel
                {
                    DetailsPatient = patient,
                    GenderTypes = genderTypes,
                    BloodTypes = bloodTypes,
                    DocumentTypes = documentTypes,
                    CivilTypes = civilTypes,
                    ProfessionalTrainingTypes = professionalTrainingTypes,
                    SureHealthTypes = sureHealthTypes,
                    Countries = countries,
                    Provinces = provinces,
                    Parents = parents,
                    AllergiesTypes = allergies,
                    SurgeriesTypes = surgeries,
                    FamilyMember  = familyMember

                };

                // Retornar la vista con el modelo
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Unexpected error: " + ex.Message;
                return View();
            }
        }



    }
}
