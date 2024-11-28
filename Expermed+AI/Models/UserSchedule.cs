using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class UserSchedule
{
    public int ScheduleId { get; set; }

    public int UsersId { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int AppointmentInterval { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? WorksDays { get; set; }

    public virtual User Users { get; set; } = null!;
}
