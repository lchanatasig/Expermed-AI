using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class ImagesConsultation
{
    public int ImagesId { get; set; }

    public int? ImagesConsultationsid { get; set; }

    public int? ImagesImagesid { get; set; }

    public string? ImagesAmount { get; set; }

    public string? ImagesObservation { get; set; }

    public int? ImagesSequential { get; set; }

    public int? ImagesStatus { get; set; }

    public virtual Consultation? ImagesConsultations { get; set; }

    public virtual Image? ImagesImages { get; set; }
}
