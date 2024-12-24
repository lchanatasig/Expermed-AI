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

                // Validar si la cita no existe
                if (appointment == null)
                {
                    return NotFound("Appointment Not Found");
                }

                // Redirigir a la lista de citas si se encuentra la cita
                return RedirectToAction("AppointmentList", "Appointment");
            }
            catch (Exception ex)
            {
                // Registrar el error y devolver un mensaje de error genérico
                // Puedes usar un sistema de logging como Serilog, NLog, etc.
                Console.Error.WriteLine($"Error fetching appointment: {ex.Message}");
                return StatusCode(500, "An error occurred while processing your request.");
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


    }
}
