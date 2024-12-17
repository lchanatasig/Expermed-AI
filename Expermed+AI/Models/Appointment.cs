using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public DateTime? AppointmentCreatedate { get; set; }

    public DateTime? AppointmentModifydate { get; set; }

    public int? AppointmentCreateuser { get; set; }

    public int? AppointmentModifyuser { get; set; }

    public DateTime AppointmentDate { get; set; }

    public TimeOnly AppointmentHour { get; set; }

    public int? AppointmentPatientid { get; set; }

    public int? AppointmentStatus { get; set; }

    public virtual User? AppointmentCreateuserNavigation { get; set; }

    public virtual User? AppointmentModifyuserNavigation { get; set; }

    public virtual Patient? AppointmentPatient { get; set; }
}
