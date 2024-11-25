using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class UserSchedule
{
    public int ScheduleId { get; set; }

    public int UsersId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string WorkDays { get; set; } = null!;

    public int AppointmentInterval { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User Users { get; set; } = null!;
}
