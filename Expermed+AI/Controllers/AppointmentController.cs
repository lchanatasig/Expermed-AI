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



        [HttpGet]
        public async Task<IActionResult> AppointmentGetById(int id)
        {
            try
            {
                var appointment = _appointmentService.GetAppointmentById(id);

                if (appointment == null)
                {
                    return NotFound(new { message = "Appointment Not Found" });
                }

                // Devuelve la información de la cita como JSON
                return Json(new
                {
                    Patient = appointment.AppointmentPatientid,
                    Date = appointment.AppointmentDate.ToString("yyyy-MM-dd"),
                    Time = appointment.AppointmentHour.ToString("HH:mm")
                });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching appointment: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while processing your request." });
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
                    TempData["ErrorMessage"] = "No available hours for the selected date.";  // Almacenar el mensaje en TempData
                    return NoContent();  // Si no hay horas disponibles, devolver un estado 204 No Content
                }

                return Ok(availableHours);  // Si hay horas disponibles, devolverlas con un estado 200 OK
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";  // Almacenar el mensaje de error en TempData
                return StatusCode(500, new { message = ex.Message });  // Manejo de errores en caso de fallos en el servicio
            }
        }



        // En el controlador de tu backend
        [HttpPost("CreateAppointment")]
        public async Task<IActionResult> CreateAppointment([FromBody] Appointment request)
        {
            try
            {
                var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

                // Lógica para crear la cita
                var appointment = new Appointment
                {
                    AppointmentCreatedate = DateTime.Now,
                    AppointmentModifydate = DateTime.Now,
                    AppointmentCreateuser = usuarioId,
                    AppointmentModifyuser = usuarioId,
                    AppointmentDate = request.AppointmentDate,
                    AppointmentHour = request.AppointmentHour,
                    AppointmentPatientid = request.AppointmentPatientid,
                    AppointmentStatus = 1
                };

                await _appointmentService.CreateAppointmentAsync(appointment);

                return Ok(new { success = true, message = "Appointment created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        //Modificar una cita
        // En el controlador de tu backend
        [HttpPost("ModifyAppointment")]
        public async Task<IActionResult> ModifyAppointment([FromBody] Appointment request)
        {
            try
            {
                var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

                // Lógica para modificar la cita
                var appointment = new Appointment
                {
                    AppointmentId = request.AppointmentId,                  // ID de la cita a modificar
                    AppointmentModifydate = DateTime.Now,                   // Fecha de modificación
                    AppointmentModifyuser = usuarioId ?? 0,                 // Usuario que realiza la modificación
                    AppointmentDate = request.AppointmentDate,              // Nueva fecha de la cita
                    AppointmentHour = request.AppointmentHour,              // Nueva hora de la cita
                    AppointmentPatientid = request.AppointmentPatientid,    // ID del paciente
                    AppointmentStatus = request.AppointmentStatus ?? 1      // Estado de la cita (por defecto 1 si no se especifica)
                };

                await _appointmentService.ModifyAppointmentAsync(appointment);

                return Ok(new { success = true, message = "Appointment modified successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("desactivate")]
        public IActionResult DesactivateAppointment([FromBody] Appointment request)
        {
            // Validar que la petición sea correcta
            if (request.AppointmentId <= 0 || request.AppointmentModifyuser <= 0)
            {
                return BadRequest(new { message = "Los parámetros proporcionados no son válidos." });
            }

            try
            {
                // Llamar al servicio para desactivar la cita
                _appointmentService.DesactivateAppointment(request.AppointmentId, request.AppointmentModifyuser ?? 0);

                // Retornar una respuesta exitosa en formato JSON
                return Ok(new { message = "Cita desactivada correctamente." });
            }
            catch (Exception ex)
            {
                // En caso de error, devolver mensaje de error en formato JSON
                return StatusCode(500, new { message = $"Error al desactivar la cita: {ex.Message}" });
            }
        }

        [HttpGet("appointmentsfortoday")]
        public async Task<IActionResult> GetAppointmentsForToday([FromQuery] int userProfile, [FromQuery] int userId)
        {
            try
            {
                var appointments = await _appointmentService.GetAppointmentsForToday(userProfile, userId);

                // Transformar las citas a un formato adecuado para el frontend
                var notifications = appointments.AsEnumerable().Select(row => new
                {
                    AppointmentId = row["appointment_id"],
                    AppointmentDate = row["appointment_date"],
                    Time = row["appointment_hour"],
                    PatientId = row["appointment_patientid"],
                    Status = row["appointment_status"],
                    TimeAgo = "Just now"  // Este campo se puede calcular dependiendo de la diferencia entre la fecha actual y la cita
                }).ToList();

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

    }
}
