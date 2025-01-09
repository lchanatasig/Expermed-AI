using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class LaboratoryConsultation
{
    public int LaboratoryId { get; set; }

    public int? LaboratoryConsultationid { get; set; }

    public int? LaboratoryLaboratoryid { get; set; }

    public string? LaboratoryAmount { get; set; }

    public string? LaboratoryObservation { get; set; }

    public int? LaboratorySequential { get; set; }

    public int? LaboatoryStatus { get; set; }

    public virtual Consultation? LaboratoryLaboratory { get; set; }

    public virtual Laboratory? LaboratoryLaboratoryNavigation { get; set; }
}
