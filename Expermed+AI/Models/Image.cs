using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class Image
{
    public int ImagesId { get; set; }

    public string ImagesName { get; set; } = null!;

    public string? ImagesDescription { get; set; }

    public string? ImagesCategory { get; set; }

    public string ImagesCie10 { get; set; } = null!;

    public int? ImagesStatus { get; set; }

    public virtual ICollection<ImagesConsultation> ImagesConsultations { get; set; } = new List<ImagesConsultation>();
}
