using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class SurgerisConsultation
{
    public int SurgeriesId { get; set; }

    public int? SurgeriesConsultationid { get; set; }

    public DateTime? SurgeriesCreationdate { get; set; }

    public int? SurgeriesCatalogid { get; set; }

    public string? SurgeriesObservation { get; set; }

    public int? SurgeriesStatus { get; set; }

    public virtual Catalog? SurgeriesCatalog { get; set; }

    public virtual Consultation? SurgeriesConsultation { get; set; }
}
