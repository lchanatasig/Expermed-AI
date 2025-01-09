using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class Specialty
{
    public int SpecialityId { get; set; }

    public string SpecialityName { get; set; } = null!;

    public string? SpecialityDescription { get; set; }

    public string? SpecialityCategory { get; set; }

    public int SpecialityStatus { get; set; }

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
