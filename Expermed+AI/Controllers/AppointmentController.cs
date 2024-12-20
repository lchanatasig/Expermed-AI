using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Expermed_AI.Models;
using Expermed_AI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

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



        public IActionResult AppointmentList()
        {
            return View();
        }





    }
}
