using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class Laboratory
{
    public int LaboratoriesId { get; set; }

    public string LaboratoriesName { get; set; } = null!;

    public string? LaboratoriesDescription { get; set; }

    public string? LaboratoriesCategory { get; set; }

    public string LaboratoriesCie10 { get; set; } = null!;

    public int? LaboratoriesStatus { get; set; }

    public virtual ICollection<LaboratoryConsultation> LaboratoryConsultations { get; set; } = new List<LaboratoryConsultation>();
}
