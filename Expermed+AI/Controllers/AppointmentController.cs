using Expermed_AI.Models;
using Expermed_AI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Expermed_AI.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly AppointmentService _appointmentService;

        public AppointmentController(ILogger<AppointmentController> logger, AppointmentService appointmentService)
        {
            _logger = logger;
            _appointmentService = appointmentService;
        }
        public class ErrorViewModel
        {
            public string Message { get; set; }
        }


        [HttpGet]
        public async Task<IActionResult> AppointmentList(int appointmentStatus = 1, int? doctorId = null)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UsuarioId");
                var userProfile = HttpContext.Session.GetInt32("PerfilId");

                if (!userId.HasValue || !userProfile.HasValue)
                {
                    return RedirectToAction("Login", "Account");
                }

                ViewBag.CurrentStatus = appointmentStatus;
                ViewBag.UserProfile = userProfile.Value;
                ViewBag.UserId = userId.Value;
                ViewBag.DoctorId = doctorId;

                var (appointments, error) = await _appointmentService.GetAppointments(
                    appointmentStatus,
                    userProfile.Value,
                    userId.Value,
                    doctorId
                );

                if (!string.IsNullOrEmpty(error))
                {
                    _logger.LogError($"Error fetching appointments: {error}");
                    TempData["Error"] = error;
                    return View(new List<Appointment>());
                }

                return View(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unhandled exception: {ex}");
                TempData["Error"] = "An unexpected error occurred";
                return View(new List<Appointment>());
            }
        }


        [HttpGet("available-hours")]
        public IActionResult GetAvailableHours([FromQuery] int userId, [FromQuery] DateTime date)
        {
            try
            {
                List<string> availableHours = _appointmentService.GetAvailableHours(userId, date);

                if (availableHours.Count == 0)
                {
                    return NoContent();  // Si no hay horas disponibles, devolver un estado 204 No Content
                }

                return Ok(availableHours);  // Si hay horas disponibles, devolverlas con un estado 200 OK
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });  // Manejo de errores en caso de fallos en el servicio
            }
        }


        [HttpPost]
        public IActionResult CreateAppointment([FromBody] Appointment request)
        {
            try
            {
                var userId = HttpContext.Session.GetInt32("UsuarioId");

                if (!userId.HasValue)
                {
                    TempData["ErrorMessage"] = "No se pudo obtener el ID del usuario. Por favor, inicie sesión.";
                    return RedirectToAction("Login", "Account");
                }

                if (string.IsNullOrEmpty(request.AppointmentHour) || request.AppointmentDate == DateTime.MinValue || request.AppointmentPatientid == 0)
                {
                    TempData["ErrorMessage"] = "Faltan datos necesarios para la cita.";
                    return RedirectToAction("PatientList", "Patient");
                }

                // Convertir la hora de la cita (appointmentTime) de string a TimeOnly
                TimeOnly appointmentHour = TimeOnly.Parse(request.AppointmentHour);

                // Crear el modelo de la cita
                Appointment appointment = new Appointment
                {
                    AppointmentCreatedate = DateTime.Now,
                    AppointmentModifydate = DateTime.Now,
                    AppointmentCreateuser = userId.Value,
                    AppointmentModifyuser = userId.Value,
                    AppointmentDate = request.AppointmentDate.Date,
                    AppointmentHour = appointmentHour,
                    AppointmentPatientid = request.AppointmentPatientid,
                    AppointmentStatus = 1
                };

                bool result = _appointmentService.CreateAppointment(appointment);

                if (result)
                {
                    TempData["SuccessMessage"] = "Cita creada exitosamente.";
                    return RedirectToAction("AppointmentList");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al crear la cita. Inténtalo de nuevo.";
                }

                return RedirectToAction("PatientList", "Patient");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
                return RedirectToAction("PatientList", "Patient");
            }
        }



    }
}
