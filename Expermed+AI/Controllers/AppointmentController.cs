using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Expermed_AI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Expermed_AI.Controllers
{
  
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly AppointmentService _appointmentService;

        public AppointmentController(ILogger<AppointmentController> logger)
        {
            _logger = logger;
        }

        public IActionResult AppointmentList()
        {
            return View();
        }

        [HttpGet("available-hours")]
        public async Task<IActionResult> GetAvailableHours(int userId, DateTime date)
        {
            try
            {
                var availableHours = await _appointmentService.GetAvailableHoursAsync(userId, date);

                if (availableHours == null || !availableHours.Any())
                {
                    return NotFound("No available hours found.");
                }

                return Ok(availableHours.Select(h => h.ToString(@"hh\:mm")));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}