using System;
using System.Collections.Generic;

namespace Expermed_AI.Models;

public partial class Province
{
    public int ProvinceId { get; set; }

    public string ProvinceName { get; set; } = null!;

    public string? ProvinceDemonym { get; set; }

    public string? ProvinvePrefix { get; set; }

    public string? ProvinceCode { get; set; }

    public string? ProvinceIso { get; set; }

    public int ProvinceStatus { get; set; }

    public int? ProvinceCountryid { get; set; }

    public virtual Country? ProvinceCountry { get; set; }
}
